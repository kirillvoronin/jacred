using System;
using System.IO;
using System.Linq;
using JacRed.Infrastructure.Persistence;
using JacRed.Models.Details;
using Xunit;

namespace JacRed.Tests.Rutracker;

[Collection(RutrackerDumpFileDbCollection.Name)]
public class RutrackerTopicIdMatchTests : IDisposable
{
    const string Name = "Тема по номеру";
    const string Original = "Topic By Number";
    readonly string _key = FileDB.KeyForTorrent(Name, Original);

    public void Dispose()
    {
        using (var fdb = FileDB.OpenWrite(_key))
        {
            fdb.Database.Clear();
            fdb.savechanges = true;
        }

        FileDB.RemoveKeyFromMasterDb(_key);
        string path = FileDB.PathForKey(_key);
        if (File.Exists(path))
            File.Delete(path);
    }

    static TorrentDetails Topic(string host, int sid) => new()
    {
        trackerName = "rutracker",
        types = new[] { "movie" },
        url = $"https://{host}/forum/viewtopic.php?t=990000501",
        title = "Тема по номеру / Topic By Number (Реж) [2011, драма, BDRip]",
        name = Name,
        originalname = Original,
        magnet = "magnet:?xt=urn:btih:CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC3",
        sizeName = "1.76 GB",
        sid = sid,
        createTime = new DateTime(2020, 1, 1),
        updateTime = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc)
    };

    [Fact]
    public void SameTopicUnderAnotherDomain_IsOneRecord()
    {
        using (var fdb = FileDB.OpenWrite(_key))
            fdb.AddOrUpdate(Topic("rutracker.net", 3));

        using (var fdb = FileDB.OpenWrite(_key))
            fdb.AddOrUpdate(Topic("rutracker.org", 7));

        var db = FileDB.OpenRead(_key, cache: false);
        var rutracker = db.Values.Where(t => t.trackerName == "rutracker").ToList();
        Assert.Single(rutracker);
        Assert.Equal("https://rutracker.org/forum/viewtopic.php?t=990000501", rutracker[0].url);
        Assert.Equal(7, rutracker[0].sid);
    }

    [Fact]
    public void DifferentTopics_StayApart()
    {
        using (var fdb = FileDB.OpenWrite(_key))
        {
            fdb.AddOrUpdate(Topic("rutracker.org", 1));
            var other = Topic("rutracker.org", 2);
            other.url = "https://rutracker.org/forum/viewtopic.php?t=990000502";
            fdb.AddOrUpdate(other);
        }

        Assert.Equal(2, FileDB.OpenRead(_key, cache: false).Count);
    }
}
