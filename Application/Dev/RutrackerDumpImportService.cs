using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JacRed.Infrastructure.Persistence;
using JacRed.Infrastructure.Trackers;
using JacRed.Infrastructure.Trackers.Rutracker;

namespace JacRed.Application.Dev
{
    public sealed class RutrackerDumpImportService
    {
        readonly TrackerWorkFlag _workFlag = new TrackerWorkFlag();
        readonly int _saveEvery;
        readonly int _batchSize;
        readonly TimeZoneInfo _zone;
        readonly Action _save;

        volatile bool _running;
        DateTime? _startedAtUtc;
        string _path;
        long _read;
        long _written;
        RutrackerDumpImportReport _last;

        public RutrackerDumpImportService()
            : this(10_000, TimeZoneInfo.Local, FileDB.SaveChangesToFile)
        {
        }

        internal RutrackerDumpImportService(int saveEvery, TimeZoneInfo zone, Action save, int batchSize = 500)
        {
            _saveEvery = saveEvery;
            _zone = zone;
            _save = save;
            _batchSize = batchSize;
        }

        public string Start(string path, DateTime dumpDateUtc)
        {
            if (string.IsNullOrWhiteSpace(path))
                return "path required";

            return TrackerSyncHelpers.RunInBackground(
                "rutracker-dump",
                "ImportDump",
                _workFlag,
                checkDisabled: false,
                ct =>
                {
                    Execute(path, dumpDateUtc, ct);
                    return Task.CompletedTask;
                });
        }

        void Execute(string path, DateTime dumpDateUtc, CancellationToken ct)
        {
            _running = true;
            _startedAtUtc = DateTime.UtcNow;
            _path = path;
            Interlocked.Exchange(ref _read, 0);
            Interlocked.Exchange(ref _written, 0);

            RutrackerDumpImportReport report;
            long savedAt = 0;

            try
            {
                using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 1 << 16);
                var importer = new RutrackerDumpImporter(dumpDateUtc, _zone)
                {
                    BatchSize = _batchSize,
                    AfterFlush = r =>
                    {
                        Interlocked.Exchange(ref _read, r.read);
                        Interlocked.Exchange(ref _written, r.written);
                        if (r.written - savedAt >= _saveEvery)
                        {
                            _save();
                            savedAt = r.written;
                        }
                    }
                };

                report = importer.Run(stream, ct);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                report = new RutrackerDumpImportReport { status = "error", dumpDate = dumpDateUtc, error = ex.Message };
            }
            catch (OperationCanceledException)
            {
                report = new RutrackerDumpImportReport
                {
                    status = "cancelled",
                    dumpDate = dumpDateUtc,
                    read = Interlocked.Read(ref _read),
                    written = Interlocked.Read(ref _written)
                };
            }

            _save();
            _last = report;
            _running = false;
        }

        public object Status()
        {
            return new
            {
                ok = true,
                running = _running,
                path = _path,
                startedAt = _startedAtUtc,
                progress = _running
                    ? new { read = Interlocked.Read(ref _read), written = Interlocked.Read(ref _written) }
                    : null,
                last = _last
            };
        }
    }
}
