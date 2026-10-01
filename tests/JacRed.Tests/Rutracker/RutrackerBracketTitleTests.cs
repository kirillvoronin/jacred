using System;
using JacRed.Infrastructure.Trackers.Rutracker;
using Xunit;

namespace JacRed.Tests.Rutracker;

public class RutrackerBracketTitleTests
{
    static (string name, string skip) Parse(string forum, string title)
    {
        var r = new RutrackerDumpRecord(1, forum, "1FB85180AAD0193F20033B28A939B4C785E16A8C", 1000, 1656069480, false, title);
        var t = RutrackerParser.FromDumpRecord(r, DateTime.UtcNow, TimeZoneInfo.Utc, out string skip);
        return (t?.name, skip);
    }

    [Theory]
    [InlineData("181", "[БЕЗ ПЕРЕВОДА] Роман о полосатом коте / Román mourovatého kocoura (Ярослав Бочек / Jaroslav Boček) [1977, Чехословакия, мультфильм для взрослых, TVRip] Original Ces", "Роман о полосатом коте")]
    [InlineData("166", "[БЕЗ ПЕРЕВОДА]Трое под простыней / Tre sotto il lenzuolo (Доменико Паолелла / Domenico Paolella, Мишель Массимо Тарантини / Michele Massimo Tarantini) [1979, Италия, Комедия, TVRip] + Original Ita", "Трое под простыней")]
    [InlineData("671", "(Леонид Филатов) Сказ про быль Леонида Филатова (Галина Коршикова) [2006, Биографический, TVRip]", "Сказ про быль Леонида Филатова")]
    [InlineData("2093", "(Не)жданный принц / Un prince (presque) charmant (Филипп Леллуш / Philippe Lellouche) [2013, Франция, мелодрама, комедия, HDTVRip] Dub", "Нежданный принц")]
    [InlineData("1950", "(Не)идеальные парочки / Per tutta la vita (Паоло Костелла / Paolo Costella) [2021, Италия, комедия, мелодрама, HDRip-AVC] Dub (Мосфильм-Мастер) + Sub Rus, Eng, Ita + Original Ita", "Неидеальные парочки")]
    [InlineData("1390", "(фильм 8) Наруто: Ураганные Хроники - Кровавая тюрьма / Gekijouban Naruto Shippuuden Movie 5: Blood Prison [Movie] [RUS(int)] [2011, приключения, комедия, боевые искусства, сёнэн, DVDRip] [HWP]", "Наруто: Ураганные Хроники - Кровавая тюрьма")]
    public void LeadingBracketTag_NameFromRestOfTitle(string forum, string title, string expected)
    {
        var (name, skip) = Parse(forum, title);
        Assert.Null(skip);
        Assert.Equal(expected, name);
    }

    [Fact]
    public void LeadingBracketTag_TitleKeepsTag()
    {
        var r = new RutrackerDumpRecord(1, "181", "1FB85180AAD0193F20033B28A939B4C785E16A8C", 1000, 1656069480, false, "[БЕЗ ПЕРЕВОДА] Роман о полосатом коте / Román mourovatého kocoura (Ярослав Бочек / Jaroslav Boček) [1977, Чехословакия, комедия, DVDRip]");
        var t = RutrackerParser.FromDumpRecord(r, DateTime.UtcNow, TimeZoneInfo.Utc, out _);
        Assert.StartsWith("[БЕЗ ПЕРЕВОДА] Роман", t.title);
    }

    [Fact]
    public void TitleWithoutLeadingBracket_NameUnchanged()
    {
        var (name, skip) = Parse("1950", "Пропасть (Алексей Ионов) [2026, триллер, мелодрама, WEB-DLRip]");
        Assert.Null(skip);
        Assert.Equal("Пропасть", name);
    }
}
