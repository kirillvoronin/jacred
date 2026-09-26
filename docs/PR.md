# Rutracker: все видеоразделы и карта разделов в init.yaml / all video forums and a configurable forum map

## Русский

**Что не так.** Карта разделов Rutracker (`RutrackerCategories.Map`) — жёсткий список из 242 разделов. В нём нет части
видеоразделов, и их раздачи в базу не попадают: у rutracker родительский раздел не показывает темы подразделов, поэтому
обход родителя не заменяет обход детей. Пример — «Аниме» (33) есть, а «Наруто» (1390), «Ван-Пис» (2544), «Аниме (DVD
Video)» (599), «Аниме (плеерный подраздел)» (1391), «Гандам» (1642), «Покемоны» (404), «Дунхуа и Эни» (1277) — нет.
Добавить раздел можно было только пересборкой.

**Что меняется.**

1. В карту добавлены все недостающие видеоразделы — 113 штук (~87 тыс. раздач на 26.09.2026 по
   `api.rutracker.cc/v1/static/forum_size`). Источник — официальное дерево форумов
   `https://api.rutracker.cc/v1/static/cat_forum_tree`: категории «Кино, Видео и ТВ», «Сериалы», «Документалистика и
   юмор», «Спорт» и подразделы 1202 «Фильмы и передачи по авто/мото». Видео ли раздел — по карте категорий Jackett.
   Не берутся: архивы, «Ищу», обсуждения, правила, звуковые дорожки, арт, AMV/VMV/ролики, подборки ссылок. Итог: 355
   разделов, 165 из них в часовом `parse` (фильмы, сериалы, мультфильмы, аниме — как и раньше; документалистика,
   шоу и спорт — только `UpdateTasksParse`/`ParseAllTask`).
2. Карту можно поправить в `init.yaml` без пересборки:

   ```yaml
   Rutracker:
     excludeForums: ["1392"]          # не обходить встроенные разделы
     forums:                          # добавить раздел или поменять поля встроенного
       - { id: 807, types: [tvshow], kind: nonstandard, quick: false }
       - { id: 1950, quick: false }   # у встроенного меняется только указанное
   ```

   Без ключей — встроенная карта, поведение прежнее. Новый раздел без `types` пропускается; неверный `kind` читается как
   `nonstandard`; о том и другом предупреждает проверка конфигурации (`ConfigSchema.ValidateAgainstSchema`). Карта
   пересчитывается при перезагрузке конфигурации (`AppInit.conf.Rutracker` — новый объект).
3. `RutrackerSettings : TrackerSettings` несёт `forums`/`excludeForums`; остальные трекеры не тронуты.

**Проверка.** `dotnet test tests/JacRed.Tests` — все тесты зелёные. Новые: каждый раздел дерева
(`Fixtures/Rutracker/cat_forum_tree_video.json`, снимок 26.09.2026) либо в карте, либо в явном списке «не видеораздачи»;
разбор `forums`/`excludeForums` из YAML; слияние с встроенной картой; предупреждения проверки конфигурации.

**Нагрузка.** Часовой `parse`: 165 листингов вместо 98. `UpdateTasksParse`: 355 вместо 242. Первый `ParseAllTask`
после обновления дотягивает ~87 тыс. новых тем (topic GET на каждую) — при `reqMinute: 30` это порядка двух суток
фонового обхода; дальше — как раньше. Кому не нужно — `excludeForums`.

## English

**Problem.** The Rutracker forum map (`RutrackerCategories.Map`) is a hard-coded list of 242 forums. It misses a number of
video forums, so their releases never reach the database: a Rutracker parent forum does not list its subforums' topics,
so crawling the parent does not cover the children. E.g. «Аниме» (33) is mapped while «Наруто» (1390), «Ван-Пис» (2544),
«Аниме (DVD Video)» (599), «Аниме (плеерный подраздел)» (1391), «Гандам» (1642), «Покемоны» (404) and «Дунхуа и Эни»
(1277) are not. Adding a forum required a rebuild.

**Change.**

1. The 113 missing video forums are added (~87k releases as of 2026-09-26 per `api.rutracker.cc/v1/static/forum_size`).
   Source: the official forum tree `https://api.rutracker.cc/v1/static/cat_forum_tree` — categories «Кино, Видео и ТВ»,
   «Сериалы», «Документалистика и юмор», «Спорт», plus the subforums of 1202 (auto/moto shows); whether a forum is video
   follows Jackett's category map. Excluded: archives, request/discussion/rules forums, audio tracks, artwork,
   AMV/VMV/clips, link collections. Result: 355 forums, 165 in the hourly `parse` (movies, series, cartoons, anime — as
   before; documentaries, shows and sport are only in `UpdateTasksParse`/`ParseAllTask`).
2. The map can be edited in `init.yaml` without a rebuild (`Rutracker.forums` adds a forum or overrides fields of a
   built-in one; `Rutracker.excludeForums` drops built-in ones). No keys — built-in map, unchanged behaviour. A new forum
   without `types` is skipped, an unknown `kind` reads as `nonstandard`; config validation warns about both. The map is
   recomputed on config reload.
3. `RutrackerSettings : TrackerSettings` carries `forums`/`excludeForums`; other trackers are untouched.

**Tests.** `dotnet test tests/JacRed.Tests` is green. New: every forum of the tree snapshot
(`Fixtures/Rutracker/cat_forum_tree_video.json`) is either mapped or in an explicit "not video releases" list; YAML
parsing of `forums`/`excludeForums`; merge with the built-in map; validation warnings.

**Load.** Hourly `parse`: 165 listings instead of 98; `UpdateTasksParse`: 355 instead of 242. The first `ParseAllTask`
after the update fetches ~87k new topics (one topic GET each) — about two days of background crawl at `reqMinute: 30`;
afterwards the same as before. Use `excludeForums` to opt out.
