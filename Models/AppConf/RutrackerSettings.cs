namespace JacRed.Models.AppConf
{
    public class RutrackerSettings : TrackerSettings
    {
        public RutrackerSettings(string host, bool useproxy = false, LoginSettings login = null, int reqMinute = 8)
            : base(host, useproxy, login, reqMinute)
        {
        }

        /// <summary>
        /// Разделы сверх встроенной карты или замена полей встроенного раздела (по id).
        /// Пусто — только встроенная карта (RutrackerCategories.Map).
        /// </summary>
        public RutrackerForumSettings[] forums { get; set; }

        /// <summary>Разделы встроенной карты, которые не обходить.</summary>
        public string[] excludeForums { get; set; }
    }

    public class RutrackerForumSettings
    {
        /// <summary>Номер раздела: viewforum.php?f={id}.</summary>
        public string id { get; set; }

        /// <summary>Типы раздач: movie, serial, multfilm, multserial, anime, documovie, docuserial, tvshow, sport.</summary>
        public string[] types { get; set; }

        /// <summary>Разбор названия: movie, serial или nonstandard.</summary>
        public string kind { get; set; }

        /// <summary>Обходить в часовом parse (первая страница), а не только в UpdateTasksParse/ParseAllTask.</summary>
        public bool? quick { get; set; }
    }
}
