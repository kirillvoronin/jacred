using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using JacRed.Configuration;
using JacRed.Configuration.Schema;
using JacRed.Infrastructure.Trackers.Rutracker;
using JacRed.Models.AppConf;
using Xunit;

namespace JacRed.Tests.Rutracker;

public class RutrackerForumsConfigTests
{
    /// <summary>
    /// Разделы дерева форумов, где нет видеораздач: архивы, «Ищу», обсуждения,
    /// правила, звуковые дорожки, арт, ролики, подборки ссылок.
    /// </summary>
    static readonly HashSet<string> NotVideoReleases = new()
    {
        "756", "214", "692", "1253", "23", "707", "1194", "772", "789", "65", "267", "1640", "185", "1454", "2374",
        "1692", "2373", "254", "771", "44", "906", "69", "149", "125", "186", "531", "540", "513", "96", "94", "653",
        "2344", "2097", "86", "665", "932", "931", "1385", "705", "809", "2484", "1386", "1387", "535", "917", "26",
        "32", "67", "1147", "191", "190", "2369", "2406", "914", "913", "2102", "2101", "1629", "2172", "73", "77",
        "891", "523", "1332", "1495", "261", "964", "1609", "1610", "1999", "2000", "1476"
    };

    [Fact]
    public void EveryVideoForumOfTheTree_IsInMap()
    {
        using var doc = JsonDocument.Parse(FixtureLoader.Read("Rutracker/cat_forum_tree_video.json"));
        var names = doc.RootElement.GetProperty("f");
        var missing = new List<string>();

        foreach (var category in doc.RootElement.GetProperty("tree").EnumerateObject())
        {
            foreach (var parent in category.Value.EnumerateObject())
            {
                var ids = new List<string> { parent.Name };
                ids.AddRange(parent.Value.EnumerateArray().Select(s => s.GetInt32().ToString()));
                foreach (string id in ids)
                {
                    if (!NotVideoReleases.Contains(id) && !RutrackerCategories.Map.ContainsKey(id))
                        missing.Add($"{id} {names.GetProperty(id).GetString()}");
                }
            }
        }

        Assert.Empty(missing);
    }

    [Fact]
    public void NotVideoReleases_AreNotInMap()
    {
        Assert.All(NotVideoReleases, id => Assert.False(RutrackerCategories.Map.ContainsKey(id), id));
    }

    [Theory]
    [InlineData("1390")]   // Наруто
    [InlineData("2544")]   // Ван-Пис
    [InlineData("599")]    // Аниме (DVD Video)
    [InlineData("1391")]   // Аниме (плеерный подраздел)
    [InlineData("1642")]   // Гандам
    [InlineData("404")]    // Покемоны
    [InlineData("1277")]   // Дунхуа и Эни
    public void AnimeSubforums_AreInQuickParse(string id)
    {
        Assert.True(RutrackerCategories.Map.TryGetValue(id, out var meta));
        Assert.Equal(new[] { "anime" }, meta.Types);
        Assert.Equal(RutrackerTitleKind.NonStandard, meta.TitleKind);
        Assert.True(meta.QuickParse);
    }

    [Fact]
    public void Resolve_WithoutSettings_IsBuiltInMap()
    {
        var map = RutrackerCategories.Resolve(null, null);
        Assert.Equal(RutrackerCategories.Map.Keys.OrderBy(k => k), map.Keys.OrderBy(k => k));
    }

    [Fact]
    public void Resolve_ExcludeForums_RemovesBuiltIn()
    {
        var map = RutrackerCategories.Resolve(null, new[] { "1392", " 24 ", "", null });
        Assert.False(map.ContainsKey("1392"));
        Assert.False(map.ContainsKey("24"));
        Assert.Equal(RutrackerCategories.Map.Count - 2, map.Count);
    }

    [Fact]
    public void Resolve_Forums_AddsNewForum()
    {
        var map = RutrackerCategories.Resolve(new[]
        {
            new RutrackerForumSettings { id = "807", types = new[] { "tvshow" }, kind = "NonStandard", quick = true }
        }, null);

        Assert.True(map.TryGetValue("807", out var meta));
        Assert.Equal(new[] { "tvshow" }, meta.Types);
        Assert.Equal(RutrackerTitleKind.NonStandard, meta.TitleKind);
        Assert.True(meta.QuickParse);
    }

    [Fact]
    public void Resolve_Forums_OverridesOnlyGivenFields()
    {
        var map = RutrackerCategories.Resolve(new[] { new RutrackerForumSettings { id = "1950", quick = false } }, null);

        Assert.True(map.TryGetValue("1950", out var meta));
        Assert.Equal(new[] { "movie" }, meta.Types);
        Assert.Equal(RutrackerTitleKind.Movie, meta.TitleKind);
        Assert.False(meta.QuickParse);
        Assert.True(RutrackerCategories.Map["1950"].QuickParse);
    }

    [Theory]
    [InlineData(null, "movie")]
    [InlineData("abc", "movie")]
    [InlineData("807", null)]
    public void Resolve_Forums_SkipsInvalidNewForum(string id, string type)
    {
        var forum = new RutrackerForumSettings { id = id, types = type == null ? null : new[] { type } };
        var map = RutrackerCategories.Resolve(new[] { forum }, null);
        Assert.Equal(RutrackerCategories.Map.Count, map.Count);
    }

    [Fact]
    public void Resolve_UnknownKind_FallsBackToNonStandard()
    {
        var map = RutrackerCategories.Resolve(new[]
        {
            new RutrackerForumSettings { id = "807", types = new[] { "tvshow" }, kind = "1" }
        }, null);

        Assert.Equal(RutrackerTitleKind.NonStandard, map["807"].TitleKind);
        Assert.False(map["807"].QuickParse);
    }

    [Fact]
    public void InitYaml_ReadsForumsAndExcludeForums()
    {
        const string yaml = """
            Rutracker:
              reqMinute: 30
              excludeForums: ["1392"]
              forums:
                - id: 807
                  types: [tvshow]
                  kind: nonstandard
                  quick: true
            """;

        var conf = AppConfigurationLoader.TryParseConfigContent(yaml, "yaml", out string error);

        Assert.Null(error);
        Assert.Equal("https://rutracker.org", conf.Rutracker.host);
        Assert.Equal(30, conf.Rutracker.reqMinute);
        Assert.Equal(new[] { "1392" }, conf.Rutracker.excludeForums);
        var forum = Assert.Single(conf.Rutracker.forums);
        Assert.Equal("807", forum.id);
        Assert.True(forum.quick);

        var map = RutrackerCategories.Resolve(conf.Rutracker.forums, conf.Rutracker.excludeForums);
        Assert.True(map.ContainsKey("807"));
        Assert.False(map.ContainsKey("1392"));
    }

    [Fact]
    public void Validator_WarnsAboutBadForums()
    {
        var conf = new AppInit();
        conf.Rutracker.forums = new[]
        {
            new RutrackerForumSettings { id = "x1" },
            new RutrackerForumSettings { id = "807" },
            new RutrackerForumSettings { id = "1950", kind = "cartoon" }
        };
        var errors = new List<string>();
        var warnings = new List<string>();

        ConfigSchema.ValidateAgainstSchema(conf, errors, warnings);

        Assert.Contains(warnings, w => w.Contains("некорректный id «x1»"));
        Assert.Contains(warnings, w => w.Contains("807") && w.Contains("нет types"));
        Assert.Contains(warnings, w => w.Contains("1950") && w.Contains("cartoon"));
    }
}
