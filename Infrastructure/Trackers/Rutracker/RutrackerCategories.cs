using System.Collections.Generic;
using System.Linq;
using JacRed.Models.AppConf;

namespace JacRed.Infrastructure.Trackers.Rutracker
{
    public enum RutrackerTitleKind
    {
        Movie,
        Serial,
        NonStandard
    }

    sealed class RutrackerCategory
    {
        public string[] Types { get; init; }

        public RutrackerTitleKind TitleKind { get; init; }

        /// <summary>Included in ParseAsync (first-page cron). Full map used by UpdateTasksParse.</summary>
        public bool QuickParse { get; init; }
    }

    static class RutrackerCategories
    {
        public static readonly Dictionary<string, RutrackerCategory> Map = new()
        {
            // 3D Кинофильмы / Наше / Зарубежное / Арт-хаус / HD Video (movie)
            ["549"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["22"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["1666"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["941"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["1950"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["2090"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["2221"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["2091"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["2092"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["2093"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["2200"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["2540"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["934"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["505"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["124"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["1457"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["2199"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["313"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["312"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["1247"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["2201"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["2339"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["140"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["252"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },

            // Фильмы: разделы UHD и DVD, тоже пропущенные (15.08.2026).
            ["7"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["718"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["1940"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["271"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["272"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["775"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["1543"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["101"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["100"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["572"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },

            // 3D Мультфильмы / Мультфильмы
            ["84"] = new() { Types = new[] { "multfilm" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["2343"] = new() { Types = new[] { "multfilm" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["930"] = new() { Types = new[] { "multfilm" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["2365"] = new() { Types = new[] { "multfilm" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["208"] = new() { Types = new[] { "multfilm" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["539"] = new() { Types = new[] { "multfilm" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["209"] = new() { Types = new[] { "multfilm" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["1213"] = new() { Types = new[] { "multfilm" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["4"] = new() { Types = new[] { "multfilm" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },
            ["1577"] = new() { Types = new[] { "multfilm" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true },

            // Мультсериалы
            ["921"] = new() { Types = new[] { "multserial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["815"] = new() { Types = new[] { "multserial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["1460"] = new() { Types = new[] { "multserial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["498"] = new() { Types = new[] { "multserial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },

            // Сериалы (зарубежные, русские, HD, LatAm, KR/JP)
            ["842"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["235"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["242"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["819"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["1531"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["721"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["1102"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["1120"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["1214"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["489"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["387"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["9"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["81"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["119"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["1803"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["266"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["193"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["1690"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["1459"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["1463"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["825"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["1248"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["1288"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["325"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["534"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["694"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["704"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["915"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = true },
            ["1939"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = true },

            // Сериальные разделы, которых в списке не было (добавлены 15.08.2026).
            //
            // Повод: раздача «Дом дракона» S3 в 2160p (t=6873874) не находилась
            // ни в вебе, ни в Лампе — она лежит в разделе 1171, а у нас был
            // только его РОДИТЕЛЬ, 119. У rutracker раздел может иметь и
            // подразделы, и собственные темы, поэтому обход родителя не
            // заменяет обход детей: в самом 119 всего два десятка тем.
            ["2366"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["189"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["1171"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["812"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["920"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["911"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["2100"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },

            // Географические UHD/HD-листы под 119 / 2100 / 2366 (добавлены 10.09.2026).
            //
            // Повод: Silo S2 UHD (t=6601495) уехала из 2366 в 1669 «Сериалы США и
            // Канады (UHD Video)». Родитель 119 в карте был, но его viewforum
            // не содержит тем детей. Jackett-имена для этих id устарели
            // (переиспользование разделов) — подписи брать с живого сайта.
            ["1669"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["2393"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["625"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["1949"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["173"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true },
            ["820"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = true },
            ["1242"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = true },
            ["717"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = true },
            ["2412"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = true },

            // Аниме
            ["1105"] = new() { Types = new[] { "anime" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = true },
            ["2491"] = new() { Types = new[] { "anime" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = true },
            ["1389"] = new() { Types = new[] { "anime" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = true },
            ["33"] = new() { Types = new[] { "anime" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = true },
            ["1106"] = new() { Types = new[] { "anime" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = true },

            // Документальные фильмы
            ["709"] = new() { Types = new[] { "documovie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = false },
            ["2109"] = new() { Types = new[] { "documovie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = false },
            ["1985"] = new() { Types = new[] { "documovie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = false },
            ["1202"] = new() { Types = new[] { "documovie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = false },

            // Документалистика
            ["46"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["671"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2177"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2538"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["251"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["98"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["97"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["851"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2178"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["821"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2076"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["56"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2123"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["876"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2139"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1467"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1469"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["249"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["552"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["500"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2112"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1327"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1468"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2168"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2160"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["314"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1281"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2110"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["979"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2169"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2164"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2166"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2163"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },

            // Развлекательные телепередачи и шоу
            ["24"] = new() { Types = new[] { "tvshow" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1959"] = new() { Types = new[] { "tvshow" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["939"] = new() { Types = new[] { "tvshow" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1481"] = new() { Types = new[] { "tvshow" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["113"] = new() { Types = new[] { "tvshow" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["115"] = new() { Types = new[] { "tvshow" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["882"] = new() { Types = new[] { "tvshow" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1482"] = new() { Types = new[] { "tvshow" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["393"] = new() { Types = new[] { "tvshow" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2537"] = new() { Types = new[] { "tvshow" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["532"] = new() { Types = new[] { "tvshow" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["827"] = new() { Types = new[] { "tvshow" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },

            // Спорт
            ["1392"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2475"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2493"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2113"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2482"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2103"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2522"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2485"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2486"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2479"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2089"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1794"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["845"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2312"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["343"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2111"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1527"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2069"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1323"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2009"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2010"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2006"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2007"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2005"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["259"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2004"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2001"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2002"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["283"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1997"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2003"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1608"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2294"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1229"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1693"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2532"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["136"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["592"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2533"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1952"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1621"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2075"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1668"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1613"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1614"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1623"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1615"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1630"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2425"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2514"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1616"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2014"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1442"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1491"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1987"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1617"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1620"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1998"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1343"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["751"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1697"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["255"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["260"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["256"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1986"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["660"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1551"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["626"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["262"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1326"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["978"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1287"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1188"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1667"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1675"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["257"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["875"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["263"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2073"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["550"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2124"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1470"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["528"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["486"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["854"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2079"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1336"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2171"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1339"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2455"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1434"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2350"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["1472"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2068"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },
            ["2016"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false },

            // Остальные видеоразделы (добавлены 26.09.2026).
            //
            // Источник — полное дерево форумов https://api.rutracker.cc/v1/static/cat_forum_tree
            // (категории «Кино, Видео и ТВ», «Сериалы», «Документалистика и юмор», «Спорт»,
            // подразделы 1202 «Авто/мото»), видео-классы — по карте категорий Jackett.
            // Не берём: архивы, «Ищу», обсуждения, правила, звуковые дорожки, арт, AMV/VMV/ролики.
            // Число раздач на 26.09.2026 (api.rutracker.cc/v1/static/forum_size): 87 260.
            // Фильмы
            ["376"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true }, // Авторские дебюты
            ["106"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true }, // Фильмы России и СССР на национальных языках [без перевода]
            ["187"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true }, // Классика мирового кинематографа
            ["212"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true }, // Сборники фильмов
            ["2459"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true }, // Короткий метр
            ["1235"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true }, // Грайндхаус
            ["166"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true }, // Зарубежные фильмы без перевода
            ["93"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true }, // DVD Video
            ["905"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true }, // Классика мирового кинематографа (DVD Video)
            ["877"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true }, // Фильмы Ближнего Зарубежья (DVD Video)
            ["1576"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true }, // Азиатские фильмы (DVD Video)
            ["2220"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true }, // Индийское кино (DVD Video)
            ["1670"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true }, // Грайндхаус (DVD Video)
            ["2198"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true }, // HD Video
            ["194"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true }, // Грайндхаус (HD Video)
            ["352"] = new() { Types = new[] { "movie" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true }, // 3D/Стерео Кино, Видео, TV и Спорт

            // Театр, юмор и телепередачи
            ["511"] = new() { Types = new[] { "tvshow" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // Театр
            ["1493"] = new() { Types = new[] { "tvshow" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // Спектакли без перевода
            ["1569"] = new() { Types = new[] { "tvshow" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // [Видео Юмор] Званый ужин
            ["373"] = new() { Types = new[] { "tvshow" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // [Видео Юмор] Хорошие Шутки
            ["1186"] = new() { Types = new[] { "tvshow" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // [Видео Юмор] Вечерний Квартал
            ["137"] = new() { Types = new[] { "tvshow" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // [Видео Юмор] Фильмы со смешным переводом (пародии)
            ["1484"] = new() { Types = new[] { "tvshow" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // [Видео Юмор] Цирк
            ["1485"] = new() { Types = new[] { "tvshow" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // [Видео Юмор] Школа злословия
            ["114"] = new() { Types = new[] { "tvshow" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // [Видео Юмор] Сатирики и юмористы
            ["1982"] = new() { Types = new[] { "tvshow" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // Развлекательные передачи

            // Спорт
            ["514"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // 3D Спорт
            ["926"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // XXV Зимние Олимпийские игры 2026
            ["1311"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // Биатлон
            ["922"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // Хоккей
            ["918"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // Керлинг
            ["1315"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // XXIV Зимние Олимпийские игры 2022
            ["1346"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // XXXIII Летние Олимпийские игры 2024
            ["2338"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // Бокс. Борьба Вольная и Греко-римская. Дзюдо. Карате. Тхэквондо
            ["927"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // Другие виды спорта
            ["1319"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // Спорт (видео)
            ["2545"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // Клубный Чемпионат Мира 2025
            ["2147"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // Чемпионат Мира 2026 (финальный турнир)
            ["497"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // Документальные фильмы (футбол)
            ["2008"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // СССР - Канада
            ["126"] = new() { Types = new[] { "sport" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // Документальные фильмы и аналитика

            // Мультфильмы
            ["1900"] = new() { Types = new[] { "multfilm" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true }, // Отечественные мультфильмы (DVD)
            ["2258"] = new() { Types = new[] { "multfilm" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true }, // Иностранные короткометражные мультфильмы (DVD)
            ["521"] = new() { Types = new[] { "multfilm" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true }, // Иностранные мультфильмы (DVD)
            ["2183"] = new() { Types = new[] { "multfilm" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true }, // Мультфильмы Ближнего Зарубежья
            ["484"] = new() { Types = new[] { "multfilm" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true }, // Иностранные короткометражные мультфильмы
            ["822"] = new() { Types = new[] { "multfilm" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true }, // Сборники мультфильмов
            ["181"] = new() { Types = new[] { "multfilm" }, TitleKind = RutrackerTitleKind.Movie, QuickParse = true }, // Мультфильмы без перевода

            // Мультсериалы
            ["816"] = new() { Types = new[] { "multserial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Мультсериалы (DVD Video)

            // Аниме (в т.ч. разделы тайтлов: Наруто, Ван-Пис, Гандам, Покемоны)
            ["599"] = new() { Types = new[] { "anime" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = true }, // Аниме (DVD Video)
            ["1391"] = new() { Types = new[] { "anime" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = true }, // Аниме (плеерный подраздел)
            ["2544"] = new() { Types = new[] { "anime" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = true }, // Ван-Пис
            ["1642"] = new() { Types = new[] { "anime" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = true }, // Гандам
            ["1390"] = new() { Types = new[] { "anime" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = true }, // Наруто
            ["404"] = new() { Types = new[] { "anime" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = true }, // Покемоны
            ["1277"] = new() { Types = new[] { "anime" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = true }, // Дунхуа и Эни

            // Сериалы (в т.ч. разделы отдельных сериалов и некондиция)
            ["80"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Сельский детектив
            ["1535"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // По законам военного времени
            ["188"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Московские тайны
            ["91"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Я знаю твои секреты
            ["990"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Универ / Универ. Новая общага / СашаТаня
            ["1408"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Женская версия
            ["175"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // След
            ["79"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Некрасивая подружка
            ["104"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Психология преступления
            ["1359"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Веб-сериалы, Вебизоды к сериалам и Пилотные серии сериалов
            ["184"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Бесстыжие / Shameless (US)
            ["1417"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Во все тяжкие / Breaking Bad
            ["1449"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Игра престолов / Game of Thrones
            ["504"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Клан Сопрано / The Sopranos
            ["372"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Сверхъестественное / Supernatural
            ["110"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Секретные материалы / The X-Files
            ["121"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Твин пикс / Twin Peaks
            ["507"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Теория большого взрыва + Детство Шелдона
            ["536"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Форс-мажоры / Костюмы в законе / Suits
            ["1144"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Ходячие мертвецы + Бойтесь ходячих мертвецов
            ["195"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Для некондиционных раздач
            ["265"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Игра престолов / Game of Thrones (HD Video)
            ["2404"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Сверхъестественное / Supernatural (HD Video)
            ["2405"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Секретные материалы / The X-Files (HD Video)
            ["2370"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Твин пикс / Twin Peaks (HD Video)
            ["2396"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Теория Большого Взрыва / The Big Bang Theory (HD Video)
            ["2398"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Ходячие мертвецы + Бойтесь ходячих мертвецов (HD Video)
            ["1498"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Для некондиционных раздач (HD Video)
            ["273"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Для некондиционных раздач (UHD Video)
            ["594"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Сериалы Венесуэлы
            ["1301"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Сериалы Индии
            ["607"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Сериалы Колумбии
            ["1574"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Сериалы Латинской Америки с озвучкой (раздачи папками)
            ["1539"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Сериалы Латинской Америки с субтитрами
            ["781"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Сериалы совместного производства
            ["1537"] = new() { Types = new[] { "serial" }, TitleKind = RutrackerTitleKind.Serial, QuickParse = true }, // Для некондиционных раздач

            // Документалистика, СМИ, религия, авто/мото передачи
            ["19"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // СМИ
            ["670"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // Вера и религия
            ["1475"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // [Видео Религия] Христианство
            ["2107"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // [Видео Религия] Ислам
            ["1453"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // [Видео Религия] Культы и новые религиозные движения
            ["294"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // [Видео Религия] Религии Индии, Тибета и Восточной Азии
            ["103"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // Документальные (DVD)
            ["656"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // [Док] Мастера искусств Театра и Кино
            ["2159"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // [Док] Музыка
            ["2380"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // [Док] Социальные ток-шоу
            ["672"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // [Док] Всё о доме, быте и дизайне
            ["1280"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // [Док] Битва экстрасенсов / Теория невероятности / Искатели / Галилео
            ["752"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // [Док] Русские сенсации / Программа Максимум / Профессия репортёр
            ["1114"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // [Док] Паранормальные явления
            ["2176"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // [Док] Разное / некондиция
            ["2323"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // Информационно-аналитические и общественно-политические (HD Video)
            ["1278"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // Биографии. Личности и кумиры (HD Video)
            ["85"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // Некондиционное видео - Документальные (HD Video)
            ["2151"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // Top Gear/Топ Гир
            ["1983"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // Тест драйв/Обзоры/Автосалоны
            ["1984"] = new() { Types = new[] { "docuserial", "documovie" }, TitleKind = RutrackerTitleKind.NonStandard, QuickParse = false }, // Тюнинг/форсаж
        };

        static readonly object _currentLock = new object();
        static RutrackerSettings _currentSource;
        static IReadOnlyDictionary<string, RutrackerCategory> _current = Map;

        /// <summary>Карта с учётом Rutracker.forums / Rutracker.excludeForums из init.yaml.</summary>
        public static IReadOnlyDictionary<string, RutrackerCategory> Current
        {
            get
            {
                var settings = AppInit.conf?.Rutracker;
                lock (_currentLock)
                {
                    if (!ReferenceEquals(settings, _currentSource))
                    {
                        _current = Resolve(settings?.forums, settings?.excludeForums);
                        _currentSource = settings;
                    }

                    return _current;
                }
            }
        }

        public static IEnumerable<string> Ids => Current.Keys;

        public static IEnumerable<string> QuickParseIds =>
            Current.Where(kv => kv.Value.QuickParse).Select(kv => kv.Key);

        /// <summary>
        /// Встроенная карта минус excludeForums, плюс forums (добавить раздел или заменить поля встроенного).
        /// Незаполненные поля записи берутся из встроенного раздела; новый раздел без types пропускается.
        /// </summary>
        public static Dictionary<string, RutrackerCategory> Resolve(IEnumerable<RutrackerForumSettings> forums, IEnumerable<string> excludeForums)
        {
            var result = new Dictionary<string, RutrackerCategory>(Map);

            if (excludeForums != null)
            {
                foreach (string id in excludeForums)
                {
                    if (!string.IsNullOrWhiteSpace(id))
                        result.Remove(id.Trim());
                }
            }

            if (forums == null)
                return result;

            foreach (var forum in forums)
            {
                string id = forum?.id?.Trim();
                if (string.IsNullOrEmpty(id) || !id.All(char.IsDigit))
                    continue;

                Map.TryGetValue(id, out var builtIn);

                string[] types = forum.types?.Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Trim()).ToArray();
                if (types == null || types.Length == 0)
                    types = builtIn?.Types;

                if (types == null || types.Length == 0)
                    continue;

                result[id] = new RutrackerCategory
                {
                    Types = types,
                    TitleKind = TryParseTitleKind(forum.kind, out var kind) ? kind : builtIn?.TitleKind ?? RutrackerTitleKind.NonStandard,
                    QuickParse = forum.quick ?? builtIn?.QuickParse ?? false
                };
            }

            return result;
        }

        public static bool TryParseTitleKind(string value, out RutrackerTitleKind kind)
        {
            kind = RutrackerTitleKind.NonStandard;
            return !string.IsNullOrWhiteSpace(value)
                && !value.Trim().All(char.IsDigit)
                && System.Enum.TryParse(value.Trim(), ignoreCase: true, out kind);
        }
    }
}
