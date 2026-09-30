using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JacRed.Application.Dev;
using JacRed.Infrastructure.Persistence;
using JacRed.Infrastructure.Security;
using JacRed.Infrastructure.Trackers.Rutracker;
using Xunit;

namespace JacRed.Tests.Rutracker;

[Collection(RutrackerDumpFileDbCollection.Name)]
public class RutrackerDumpImportServiceTests : IDisposable
{
    static readonly DateTime DumpDate = new(2026, 9, 27, 0, 0, 0, DateTimeKind.Utc);

    readonly List<string> _keys = new();
    readonly string _dir = Path.Combine(Path.GetTempPath(), "rtdump-" + Guid.NewGuid().ToString("N"));

    public RutrackerDumpImportServiceTests()
    {
        _ = AppInit.conf.Rutracker.host;
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        foreach (var key in _keys.Distinct())
        {
            using (var fdb = FileDB.OpenWrite(key))
            {
                fdb.Database.Clear();
                fdb.savechanges = true;
            }

            FileDB.RemoveKeyFromMasterDb(key);
            string path = FileDB.PathForKey(key);
            if (File.Exists(path))
                File.Delete(path);
        }

        try { Directory.Delete(_dir, true); } catch { }
    }

    string Movies(int first, int count)
    {
        var sb = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<torrents>\n");
        for (int i = 0; i < count; i++)
        {
            int id = first + i;
            string name = $"Сервис тест {id}";
            string orig = $"Service Test {id}";
            _keys.Add(FileDB.KeyForTorrent(name, orig));
            sb.Append($"<torrent id=\"{id}\" registred_at=\"x\" unixts=\"1577836800\" size=\"1885083648\">")
              .Append($"<title>{name} / {orig} (Реж) [2011, драма, BDRip]</title>")
              .Append($"<torrent hash=\"{id:X40}\" tracker_id=\"1\"/><forum id=\"313\">f</forum></torrent>\n");
        }

        return sb.Append("</torrents>\n").ToString();
    }

    static RutrackerDumpImportReport WaitDone(RutrackerDumpImportService service)
    {
        var clock = Stopwatch.StartNew();
        while (clock.Elapsed < TimeSpan.FromSeconds(20))
        {
            dynamic status = service.Status();
            if (!(bool)status.running && status.last != null)
                return (RutrackerDumpImportReport)status.last;
            Thread.Sleep(20);
        }

        throw new TimeoutException("import did not finish");
    }

    string Fifo()
    {
        string path = Path.Combine(_dir, "dump.fifo");
        Process.Start("mkfifo", path)!.WaitForExit();
        return path;
    }

    [Fact]
    public void MissingFile_ErrorNothingWritten()
    {
        var service = new RutrackerDumpImportService(10_000, TimeZoneInfo.Utc, () => { });

        Assert.Equal("ok", service.Start(Path.Combine(_dir, "none.xml"), DumpDate));
        var report = WaitDone(service);

        Assert.Equal("error", report.status);
        Assert.Equal(0, report.written);
    }

    [Fact]
    public void SaveEveryN_MasterDbPersistedDuringImport()
    {
        int saves = 0;
        string file = Path.Combine(_dir, "five.xml");
        File.WriteAllText(file, Movies(990000301, 5));
        var service = new RutrackerDumpImportService(2, TimeZoneInfo.Utc, () => Interlocked.Increment(ref saves), batchSize: 1);

        service.Start(file, DumpDate);
        var report = WaitDone(service);

        Assert.Equal(5, report.written);
        Assert.Equal(3, saves);
    }

    [Fact]
    public void SecondStart_WhileRunning_IsWork_AndNeverRuns()
    {
        if (!OperatingSystem.IsLinux())
            return;

        string fifo = Fifo();
        var service = new RutrackerDumpImportService(10_000, TimeZoneInfo.Utc, () => { });

        Assert.Equal("ok", service.Start(fifo, DumpDate));
        Assert.Equal("work", service.Start(fifo, DumpDate));
        File.WriteAllText(fifo, Movies(990000311, 2));
        var report = WaitDone(service);

        Assert.Equal(2, report.read);
        Assert.Equal(2, report.written);
        Thread.Sleep(200);
        dynamic status = service.Status();
        Assert.False((bool)status.running);
        Assert.Equal(2, ((RutrackerDumpImportReport)status.last).read);
    }

    [Fact]
    public void Fifo_ImportsLikeAFile()
    {
        if (!OperatingSystem.IsLinux())
            return;

        string fifo = Fifo();
        var service = new RutrackerDumpImportService(10_000, TimeZoneInfo.Utc, () => { });

        service.Start(fifo, DumpDate);
        var writer = Task.Run(() => File.WriteAllText(fifo, Movies(990000321, 3)));
        var report = WaitDone(service);
        writer.Wait();

        Assert.Equal("ok", report.status);
        Assert.Equal(3, report.written);
    }

    [Fact]
    public void Report_SerializesItsCounters()
    {
        var report = new RutrackerDumpImportReport { status = "ok", written = 7, read = 9, lastId = 42 };

        string json = System.Text.Json.JsonSerializer.Serialize(report);

        Assert.Contains("\"written\":7", json);
        Assert.Contains("\"read\":9", json);
        Assert.Contains("\"lastId\":42", json);
        Assert.Contains("\"status\":\"ok\"", json);
    }

    [Fact]
    public void Route_IsDevAdmin_AndRegistryConsistent()
    {
        Assert.Equal(JacRedAccessPolicy.DevAdmin, JacRedEndpointRegistry.ResolvePolicy("/dev/ImportRutrackerDump"));
        Assert.Equal(JacRedAccessPolicy.DevAdmin, JacRedEndpointRegistry.ResolvePolicy("/dev/ImportRutrackerDumpStatus"));
        Assert.Contains(JacRedAccessCatalog.Routes, r => r.Path == "/dev/ImportRutrackerDump");
        Assert.Empty(JacRedAccessCatalog.VerifyRegistry());
    }
}
