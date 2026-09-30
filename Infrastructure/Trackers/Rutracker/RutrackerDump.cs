using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace JacRed.Infrastructure.Trackers.Rutracker
{
    public sealed record RutrackerDumpRecord(int Id, string ForumId, string Hash, long Size, long UnixTs, bool Deleted, string Title);

    public static class RutrackerDumpReader
    {
        public static IEnumerable<RutrackerDumpRecord> Read(Stream xml, Action<string> onMalformed)
        {
            var settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Ignore,
                IgnoreWhitespace = true,
                IgnoreComments = true,
                CloseInput = false
            };

            using var reader = XmlReader.Create(xml, settings);

            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.Element || reader.Depth != 1 || reader.Name != "torrent")
                    continue;

                string idAttr = reader.GetAttribute("id");
                string sizeAttr = reader.GetAttribute("size");
                string unixAttr = reader.GetAttribute("unixts");

                string title = null, hash = null, forumId = null;
                bool deleted = false;

                using (var record = reader.ReadSubtree())
                {
                    record.Read();
                    record.Read();
                    while (!record.EOF)
                    {
                        if (record.NodeType != XmlNodeType.Element || record.Depth != 1)
                        {
                            record.Read();
                            continue;
                        }

                        switch (record.Name)
                        {
                            case "title":
                                title = record.ReadElementContentAsString();
                                continue;
                            case "torrent":
                                hash = record.GetAttribute("hash");
                                break;
                            case "forum":
                                forumId = record.GetAttribute("id");
                                break;
                            case "del":
                                deleted = true;
                                break;
                        }

                        record.Skip();
                    }
                }

                if (!int.TryParse(idAttr, NumberStyles.None, CultureInfo.InvariantCulture, out int id)
                    || !long.TryParse(sizeAttr, NumberStyles.None, CultureInfo.InvariantCulture, out long size)
                    || !long.TryParse(unixAttr, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out long unixts))
                {
                    onMalformed?.Invoke(idAttr);
                    continue;
                }

                yield return new RutrackerDumpRecord(id, forumId, hash, size, unixts, deleted, title);
            }
        }
    }
}
