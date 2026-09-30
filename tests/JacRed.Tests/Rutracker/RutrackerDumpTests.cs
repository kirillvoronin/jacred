using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using JacRed.Infrastructure.Persistence;
using JacRed.Infrastructure.Trackers.Rutracker;
using JacRed.Models.Details;
using Xunit;

namespace JacRed.Tests.Rutracker;

[CollectionDefinition(Name)]
public class RutrackerDumpFileDbCollection
{
    public const string Name = "RutrackerDumpFileDb";
}

[Collection(RutrackerDumpFileDbCollection.Name)]
public class RutrackerDumpTests : IDisposable
{
    static readonly DateTime DumpDate = new(2026, 9, 27, 0, 0, 0, DateTimeKind.Utc);
    static readonly TimeZoneInfo Msk = TimeZoneInfo.CreateCustomTimeZone("test+3", TimeSpan.FromHours(3), "test+3", "test+3");

    readonly List<string> _keys = new();

    public RutrackerDumpTests()
    {
        _ = AppInit.conf.Rutracker.host;
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
    }

    static Stream Xml(string xml) => new MemoryStream(Encoding.UTF8.GetBytes(xml));

    static Stream Fixture(string name) => Xml(FixtureLoader.Read($"Rutracker/{name}"));

    TorrentDetails Stored(string name, string originalname, string url)
    {
        string key = FileDB.KeyForTorrent(name, originalname);
        _keys.Add(key);
        var db = FileDB.OpenRead(key, cache: false);
        return db.TryGetValue(url, out var t) ? t : null;
    }

    static string Url(int id) => $"{AppInit.conf.Rutracker.host}/forum/viewtopic.php?t={id}";

    const string HashA = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA1";
    const string HashB = "BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB2";
    const string DumpTitle = "Дамп тест / Dump Test Movie (Режиссер) [2011, драма, BDRip]";
    const string DumpName = "Дамп тест";
    const string DumpOriginal = "Dump Test Movie";

    static string Dump(params string[] records) =>
        "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<torrents>\n" + string.Join("\n", records) + "\n</torrents>";

    static string Record(int id, string hash, string forum = "313", string title = DumpTitle, bool deleted = false) =>
        $"<torrent id=\"{id}\" registred_at=\"2020.01.01 00:00:00\" unixts=\"1577836800\" size=\"1885083648\">" +
        $"<title>{title}</title><torrent hash=\"{hash}\" tracker_id=\"1\"/><forum id=\"{forum}\">f</forum>" +
        "<content>c</content>" + (deleted ? "<del/>" : "") + "</torrent>";

    void Seed(int id, string name, string originalname, string hash, int sid, DateTime updateTime, string title = "Старый заголовок")
    {
        string key = FileDB.KeyForTorrent(name, originalname);
        _keys.Add(key);
        using var fdb = FileDB.OpenWrite(key);
        fdb.AddOrUpdate(new TorrentDetails
        {
            url = Url(id),
            trackerName = "rutracker",
            types = new[] { "movie" },
            title = title,
            name = name,
            originalname = originalname,
            magnet = $"magnet:?xt=urn:btih:{hash}&tr=http://bt4.t-ru.org/ann?magnet",
            sid = sid,
            pir = 1,
            sizeName = "1.76 GB",
            createTime = new DateTime(2020, 1, 1),
            updateTime = updateTime
        });
    }

    RutrackerDumpImportReport Import(string xml) =>
        new RutrackerDumpImporter(DumpDate, Msk).Run(Xml(xml), CancellationToken.None);

    [Fact]
    public void Existing_SameHash_Untouched()
    {
        var before = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc);
        Seed(990000101, "Старое имя 101", "Old Name 101", HashA, 12, before);

        var report = Import(Dump(Record(990000101, HashA)));

        var t = Stored("Старое имя 101", "Old Name 101", Url(990000101));
        Assert.Equal(12, t.sid);
        Assert.Equal(before, t.updateTime.ToUniversalTime());
        Assert.Equal("Старый заголовок", t.title);
        Assert.Equal(0, report.written);
        Assert.Equal(1, report.skippedExisting);
        Assert.Null(Stored(DumpName, DumpOriginal, Url(990000101)));
    }

    [Fact]
    public void Existing_OtherHash_OlderThanDump_OnlyMagnetReplaced()
    {
        Seed(990000102, "Старое имя 102", "Old Name 102", HashA, 12, new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc));

        var report = Import(Dump(Record(990000102, HashB)));

        var t = Stored("Старое имя 102", "Old Name 102", Url(990000102));
        Assert.Contains(HashB, t.magnet);
        Assert.Equal(12, t.sid);
        Assert.Equal(1, t.pir);
        Assert.Equal("Старый заголовок", t.title);
        Assert.Equal(1, report.magnetReplaced);
        Assert.Equal(0, report.written);
        Assert.Null(Stored(DumpName, DumpOriginal, Url(990000102)));
    }

    [Fact]
    public void Existing_OtherHash_NewerThanDump_Untouched()
    {
        Seed(990000103, "Старое имя 103", "Old Name 103", HashA, 12, new DateTime(2026, 9, 28, 0, 0, 0, DateTimeKind.Utc));

        var report = Import(Dump(Record(990000103, HashB)));

        var t = Stored("Старое имя 103", "Old Name 103", Url(990000103));
        Assert.Contains(HashA, t.magnet);
        Assert.Equal(0, report.magnetReplaced);
        Assert.Equal(1, report.skippedExisting);
    }

    [Fact]
    public void WrittenAfterIndex_BeforeImport_NotOverwritten()
    {
        var index = RutrackerExistingIndex.Build(CancellationToken.None);
        Seed(990000104, DumpName, DumpOriginal, HashA, 5, new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc), DumpTitle);

        var report = new RutrackerDumpImporter(DumpDate, Msk).Run(Xml(Dump(Record(990000104, HashA))), index, CancellationToken.None);

        var t = Stored(DumpName, DumpOriginal, Url(990000104));
        Assert.Equal(5, t.sid);
        Assert.Equal(0, report.written);
        Assert.Equal(1, report.skippedExisting);
    }

    [Fact]
    public void RecordOnDisk_KeyMissingFromMasterDb_KeyRestored()
    {
        Seed(990000105, DumpName, DumpOriginal, HashA, 7, new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc), DumpTitle);
        string key = FileDB.KeyForTorrent(DumpName, DumpOriginal);
        FileDB.RemoveKeyFromMasterDb(key);

        var report = Import(Dump(Record(990000105, HashA)));

        Assert.True(FileDB.masterDb.ContainsKey(key));
        Assert.Equal(7, Stored(DumpName, DumpOriginal, Url(990000105)).sid);
        Assert.Equal(0, report.written);
    }

    [Fact]
    public void Deleted_NewNotWritten_ExistingUntouched()
    {
        var before = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc);
        Seed(990000202, "Старое имя 202", "Old Name 202", HashA, 9, before);

        var report = Import(Dump(Record(990000201, HashA, deleted: true), Record(990000202, HashB, deleted: true)));

        Assert.Null(Stored(DumpName, DumpOriginal, Url(990000201)));
        var kept = Stored("Старое имя 202", "Old Name 202", Url(990000202));
        Assert.Contains(HashA, kept.magnet);
        Assert.Equal(9, kept.sid);
        Assert.Equal(before, kept.updateTime.ToUniversalTime());
        Assert.Equal(2, report.skippedDeleted);
        Assert.Equal(0, report.written + report.magnetReplaced);
    }

    [Fact]
    public void ForumOutsideMap_NotWritten()
    {
        var report = Import(Dump(Record(990000203, HashA, forum: "99999999")));

        Assert.Null(Stored(DumpName, DumpOriginal, Url(990000203)));
        Assert.Equal(1, report.skippedForum);
    }

    [Fact]
    public void ForumExcludedByConfig_NotWritten()
    {
        var map = RutrackerCategories.Resolve(null, new[] { "313" });

        var report = new RutrackerDumpImporter(DumpDate, Msk, map).Run(Xml(Dump(Record(990000204, HashA))), RutrackerExistingIndex.Build(CancellationToken.None), CancellationToken.None);

        Assert.Null(Stored(DumpName, DumpOriginal, Url(990000204)));
        Assert.Equal(1, report.skippedForum);
    }

    [Fact]
    public void NoHash_And_UnparsableTitle_Counted()
    {
        var report = Import(Dump(
            Record(990000205, ""),
            Record(990000206, HashA, forum: "1390", title: "Наруто Сезон 1 [2002, аниме]")));

        Assert.Equal(1, report.skippedNoHash);
        Assert.Equal(1, report.skippedTitle);
        Assert.Equal(0, report.written);
    }

    [Fact]
    public void MalformedRecord_Counted_NeighboursWritten()
    {
        string bad = "<torrent id=\"990000208\" registred_at=\"x\" unixts=\"1577836800\" size=\"много\"><title>t</title><torrent hash=\"" + HashA + "\" tracker_id=\"1\"/><forum id=\"313\">f</forum></torrent>";
        _keys.Add(FileDB.KeyForTorrent(DumpName, DumpOriginal));
        _keys.Add(FileDB.KeyForTorrent("Третий дамп", "Third Dump"));

        var report = Import(Dump(
            Record(990000207, HashA),
            bad,
            Record(990000209, HashB, title: "Третий дамп / Third Dump (Реж) [2013, драма, BDRip]")));

        Assert.Equal(1, report.failed);
        Assert.Equal(2, report.written);
        Assert.Equal("ok", report.status);
    }

    [Fact]
    public void EmptyDump_OkWithZeros()
    {
        var report = Import(Dump());

        Assert.Equal("ok", report.status);
        Assert.Equal(0, report.read);
        Assert.Equal(0, report.written);
    }

    [Fact]
    public void TruncatedXml_ErrorReported_WrittenKept()
    {
        _keys.Add(FileDB.KeyForTorrent(DumpName, DumpOriginal));
        string xml = Dump(Record(990000210, HashA)).Replace("\n</torrents>", "\n<torrent id=\"990000211\" size=\"1\" unixts=\"1\"><title>обрыв");

        var report = Import(xml);

        Assert.Equal("error", report.status);
        Assert.False(string.IsNullOrEmpty(report.error));
        Assert.Equal(990000210, report.lastId);
        Assert.Equal(1, report.written);
        Assert.NotNull(Stored(DumpName, DumpOriginal, Url(990000210)));
    }

    [Fact]
    public void SecondRun_WritesNothing()
    {
        var first = Import(Dump(Record(990000106, HashA), Record(990000107, HashB, title: "Второй дамп / Second Dump (Реж) [2012, драма, BDRip]")));
        _keys.Add(FileDB.KeyForTorrent(DumpName, DumpOriginal));
        _keys.Add(FileDB.KeyForTorrent("Второй дамп", "Second Dump"));

        var second = Import(Dump(Record(990000106, HashA), Record(990000107, HashB, title: "Второй дамп / Second Dump (Реж) [2012, драма, BDRip]")));

        Assert.Equal(2, first.written);
        Assert.Equal(0, second.written);
        Assert.Equal(first.written, second.skippedExisting);
    }

    [Fact]
    public void FormatSizeName_BinaryUnitsTwoDecimalsPlainSpace()
    {
        Assert.Equal("1.76 GB", RutrackerParser.FormatSizeName(1885083648));
        Assert.Equal("6.56 GB", RutrackerParser.FormatSizeName(7039207924));
        Assert.Equal("700.00 MB", RutrackerParser.FormatSizeName(734003200));
        Assert.Equal("1.50 TB", RutrackerParser.FormatSizeName(1649267441664));
        Assert.Equal("512.00 B", RutrackerParser.FormatSizeName(512));
    }

    [Fact]
    public void Reader_ReadsEveryFieldOfARecord()
    {
        var rec = RutrackerDumpReader.Read(Fixture("dump_small.xml"), null).Single();

        Assert.Equal(990000001, rec.Id);
        Assert.Equal("313", rec.ForumId);
        Assert.Equal("5BFBEB7C5643B9E54A66179463A1EF05B2567720", rec.Hash);
        Assert.Equal(1885083648, rec.Size);
        Assert.Equal(1368193980, rec.UnixTs);
        Assert.False(rec.Deleted);
        Assert.StartsWith("Ёлки щедрые / Yolki Dumptest", rec.Title);
    }

    [Fact]
    public void Import_NewRecord_StoredAsCrawlerWouldStoreIt()
    {
        var report = new RutrackerDumpImporter(DumpDate, Msk).Run(Fixture("dump_small.xml"), CancellationToken.None);

        var t = Stored("Елки шедрые", "Yolki Dumptest", Url(990000001));

        Assert.NotNull(t);
        Assert.Equal(1, report.written);
        Assert.Equal("rutracker", t.trackerName);
        Assert.Equal(new[] { "movie" }, t.types);
        Assert.Equal("Елки шедрые / Yolki Dumptest (Тимур Бекмамбетов) [2011, комедия, BDRip 1080p]", t.title);
        Assert.DoesNotContain("ё", t.title, StringComparison.OrdinalIgnoreCase);
        Assert.Equal("Елки шедрые", t.name);
        Assert.Equal("Yolki Dumptest", t.originalname);
        Assert.Equal(2011, t.relased);
        Assert.Equal("magnet:?xt=urn:btih:5BFBEB7C5643B9E54A66179463A1EF05B2567720&tr=http://bt4.t-ru.org/ann?magnet", t.magnet);
        Assert.Equal("1.76 GB", t.sizeName);
        Assert.DoesNotContain(' ', t.sizeName);
        Assert.True(t.size > 0);
        Assert.Equal(0, t.sid);
        Assert.Equal(0, t.pir);
        Assert.Equal(new DateTime(2013, 5, 10, 16, 53, 0), t.createTime);
        Assert.Equal(DateTimeKind.Unspecified, t.createTime.Kind);
        Assert.Equal(DumpDate, t.updateTime.ToUniversalTime());
        Assert.True(FileDB.masterDb.ContainsKey(FileDB.KeyForTorrent("Елки шедрые", "Yolki Dumptest")));
    }
}
