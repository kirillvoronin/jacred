using System.Collections.Generic;
using JacRed.Infrastructure.Trackers.Kinozal;
using JacRed.Models.AppConf;
using JacRed.Models.Details;
using Xunit;

namespace JacRed.Tests.Kinozal;

public class KinozalHostTests
{
    static TrackerSettings Mirrored() => new("https://kinozal.guru") { alias = "https://kinozal.me" };

    [Fact]
    public void Requests_GoToTheAlias()
    {
        Assert.Equal("https://kinozal.me", KinozalParser.RequestBase(Mirrored()));
    }

    [Fact]
    public void StoredUrls_UseTheCanonicalHost()
    {
        Assert.Equal("https://kinozal.guru", KinozalParser.CanonicalBase(Mirrored()));
        Assert.Equal("https://kinozal.guru/details.php?id=42",
            KinozalParser.DetailsUrl(KinozalParser.CanonicalBase(Mirrored()), 42));
    }

    [Fact]
    public void WithoutAlias_RequestsAndStorageShareTheHost()
    {
        var plain = new TrackerSettings("https://kinozal.me/");

        Assert.Equal("https://kinozal.me", KinozalParser.RequestBase(plain));
        Assert.Equal("https://kinozal.me", KinozalParser.CanonicalBase(plain));
    }

    [Fact]
    public void Cached_FoundByTopicId_UnderAnotherDomain()
    {
        var stored = new TorrentDetails { trackerName = "kinozal", url = "https://kinozal.guru/details.php?id=1686139", magnet = "magnet:?xt=urn:btih:AA" };
        var db = new Dictionary<string, TorrentDetails> { [stored.url] = stored };
        var listing = new TorrentDetails { trackerName = "kinozal", url = "https://kinozal.me/details.php?id=1686139" };

        Assert.Same(stored, KinozalParser.FindCached(db, listing));
    }

    [Fact]
    public void Cached_ExactUrlWins_AndOtherIdsAreIgnored()
    {
        var other = new TorrentDetails { trackerName = "kinozal", url = "https://kinozal.guru/details.php?id=7" };
        var same = new TorrentDetails { trackerName = "kinozal", url = "https://kinozal.me/details.php?id=8" };
        var db = new Dictionary<string, TorrentDetails> { [other.url] = other, [same.url] = same };

        Assert.Same(same, KinozalParser.FindCached(db, new TorrentDetails { trackerName = "kinozal", url = same.url }));
        Assert.Null(KinozalParser.FindCached(db, new TorrentDetails { trackerName = "kinozal", url = "https://kinozal.guru/details.php?id=9" }));
    }

    [Fact]
    public void Cached_OtherTrackerWithSameNumber_IsNotKinozal()
    {
        var rutor = new TorrentDetails { trackerName = "rutor", url = "http://rutor.info/details.php?id=5" };
        var db = new Dictionary<string, TorrentDetails> { [rutor.url] = rutor };

        Assert.Null(KinozalParser.FindCached(db, new TorrentDetails { trackerName = "kinozal", url = "https://kinozal.guru/details.php?id=5" }));
    }
}
