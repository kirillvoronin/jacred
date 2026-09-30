using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using JacRed.Infrastructure.Persistence;
using JacRed.Models.Details;

namespace JacRed.Infrastructure.Trackers.Rutracker
{
    public sealed class RutrackerDumpImportReport
    {
        public string status { get; set; }
        public DateTime dumpDate { get; set; }
        public long read { get; set; }
        public long written { get; set; }
        public long magnetReplaced { get; set; }
        public long skippedExisting { get; set; }
        public long skippedForum { get; set; }
        public long skippedDeleted { get; set; }
        public long skippedTitle { get; set; }
        public long skippedNoHash { get; set; }
        public long failed { get; set; }
        public int lastId { get; set; }
        public long seconds { get; set; }
        public string error { get; set; }
    }

    public sealed class RutrackerDumpImporter
    {
        readonly DateTime _dumpDateUtc;
        readonly TimeZoneInfo _zone;
        readonly IReadOnlyDictionary<string, RutrackerCategory> _forums;

        public int BatchSize { get; init; } = 500;

        public Action<RutrackerDumpImportReport> AfterFlush { get; init; }

        public RutrackerDumpImporter(DateTime dumpDateUtc, TimeZoneInfo zone)
            : this(dumpDateUtc, zone, null)
        {
        }

        internal RutrackerDumpImporter(DateTime dumpDateUtc, TimeZoneInfo zone, IReadOnlyDictionary<string, RutrackerCategory> forums)
        {
            _dumpDateUtc = DateTime.SpecifyKind(dumpDateUtc, DateTimeKind.Utc);
            _zone = zone ?? TimeZoneInfo.Local;
            _forums = forums;
        }

        public RutrackerDumpImportReport Run(Stream xml, CancellationToken ct) =>
            Run(xml, RutrackerExistingIndex.Build(ct), ct);

        public RutrackerDumpImportReport Run(Stream xml, RutrackerExistingIndex existing, CancellationToken ct)
        {
            var report = new RutrackerDumpImportReport { status = "ok", dumpDate = _dumpDateUtc };
            var clock = Stopwatch.StartNew();
            var batch = new List<TorrentDetails>(BatchSize);
            var forums = _forums ?? RutrackerCategories.Current;

            try
            {
                foreach (var record in RutrackerDumpReader.Read(xml, _ => report.failed++))
                {
                    ct.ThrowIfCancellationRequested();
                    report.read++;
                    report.lastId = record.Id;
                    Take(record, forums, existing, batch, report);
                }
            }
            catch (Exception ex) when (ex is XmlException || ex is IOException)
            {
                report.status = "error";
                report.error = ex.Message;
            }

            Flush(batch, report);
            report.seconds = (long)clock.Elapsed.TotalSeconds;
            return report;
        }

        void Take(RutrackerDumpRecord record, IReadOnlyDictionary<string, RutrackerCategory> forums,
            RutrackerExistingIndex existing, List<TorrentDetails> batch, RutrackerDumpImportReport report)
        {
            var torrent = RutrackerParser.FromDumpRecord(record, _dumpDateUtc, _zone, forums, out string skipReason);
            if (torrent == null)
            {
                switch (skipReason)
                {
                    case "deleted": report.skippedDeleted++; break;
                    case "forum": report.skippedForum++; break;
                    case "nohash": report.skippedNoHash++; break;
                    default: report.skippedTitle++; break;
                }

                return;
            }

            if (existing.TryGet(record.Id, out ulong knownHash, out string bucketKey, out DateTime knownUpdate))
            {
                if (knownHash != RutrackerExistingIndex.HashPrefix(record.Hash)
                    && knownUpdate < _dumpDateUtc
                    && ReplaceMagnet(bucketKey, torrent.url, torrent.magnet))
                    report.magnetReplaced++;
                else
                    report.skippedExisting++;

                return;
            }

            batch.Add(torrent);
            if (batch.Count >= BatchSize)
                Flush(batch, report);
        }

        static bool ReplaceMagnet(string bucketKey, string url, string magnet)
        {
            using var fdb = FileDB.OpenWrite(bucketKey);
            if (!fdb.Database.TryGetValue(url, out var current))
                return false;

            fdb.AddOrUpdate(new TorrentDetails
            {
                trackerName = current.trackerName,
                types = current.types,
                url = current.url,
                title = current.title,
                sid = current.sid,
                pir = current.pir,
                sizeName = current.sizeName,
                createTime = current.createTime,
                name = current.name,
                originalname = current.originalname,
                relased = current.relased,
                magnet = magnet
            });
            return true;
        }

        void Flush(List<TorrentDetails> batch, RutrackerDumpImportReport report)
        {
            if (batch.Count == 0)
                return;

            FileDB.AddOrUpdate<TorrentDetails>(batch, (t, db) =>
            {
                if (db.TryGetValue(t.url, out var already))
                {
                    string key = FileDB.KeyForTorrent(t.name, t.originalname);
                    if (!FileDB.masterDb.ContainsKey(key))
                        FileDB.SetShard(key, already.updateTime);

                    report.skippedExisting++;
                    return Task.FromResult(false);
                }

                report.written++;
                return Task.FromResult(true);
            }).AsTask().GetAwaiter().GetResult();

            batch.Clear();
            AfterFlush?.Invoke(report);
        }
    }
}
