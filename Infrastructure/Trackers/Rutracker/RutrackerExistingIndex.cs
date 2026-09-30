using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using JacRed.Infrastructure.Persistence;

namespace JacRed.Infrastructure.Trackers.Rutracker
{
    public sealed class RutrackerExistingIndex
    {
        static readonly Regex TopicIdRe = new(@"viewtopic\.php\?t=([0-9]+)", RegexOptions.Compiled);
        static readonly Regex BtihRe = new(@"urn:btih:([a-fA-F0-9]{40})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        readonly struct Entry
        {
            public readonly ulong HashPrefix;
            public readonly int Bucket;
            public readonly long UpdateTicks;

            public Entry(ulong hashPrefix, int bucket, long updateTicks)
            {
                HashPrefix = hashPrefix;
                Bucket = bucket;
                UpdateTicks = updateTicks;
            }
        }

        readonly Dictionary<int, Entry> _byId = new();
        readonly List<string> _buckets = new();

        public static RutrackerExistingIndex Build(CancellationToken ct)
        {
            var index = new RutrackerExistingIndex();

            foreach (var item in FileDB.masterDb.ToArray())
            {
                ct.ThrowIfCancellationRequested();

                int bucket = -1;
                foreach (var kv in FileDB.OpenRead(item.Key, cache: false))
                {
                    var t = kv.Value;
                    if (t == null || !string.Equals(t.trackerName, "rutracker", StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (!TryTopicId(t.url, out int id))
                        continue;

                    if (bucket < 0)
                    {
                        bucket = index._buckets.Count;
                        index._buckets.Add(item.Key);
                    }

                    var entry = new Entry(HashPrefixFromMagnet(t.magnet), bucket, t.updateTime.ToUniversalTime().Ticks);
                    if (!index._byId.TryGetValue(id, out var known) || known.UpdateTicks < entry.UpdateTicks)
                        index._byId[id] = entry;
                }
            }

            return index;
        }

        public int Count => _byId.Count;

        public bool TryGet(int id, out ulong hashPrefix, out string bucketKey, out DateTime updateTime)
        {
            if (_byId.TryGetValue(id, out var e))
            {
                hashPrefix = e.HashPrefix;
                bucketKey = _buckets[e.Bucket];
                updateTime = new DateTime(e.UpdateTicks, DateTimeKind.Utc);
                return true;
            }

            hashPrefix = 0;
            bucketKey = null;
            updateTime = default;
            return false;
        }

        internal static bool TryTopicId(string url, out int id)
        {
            id = 0;
            if (string.IsNullOrEmpty(url))
                return false;

            var m = TopicIdRe.Match(url);
            return m.Success && int.TryParse(m.Groups[1].Value, NumberStyles.None, CultureInfo.InvariantCulture, out id);
        }

        internal static ulong HashPrefix(string hash) =>
            string.IsNullOrEmpty(hash) || hash.Length < 16
                ? 0
                : ulong.Parse(hash.AsSpan(0, 16), NumberStyles.HexNumber, CultureInfo.InvariantCulture);

        static ulong HashPrefixFromMagnet(string magnet)
        {
            if (string.IsNullOrEmpty(magnet))
                return 0;

            var m = BtihRe.Match(magnet);
            return m.Success ? HashPrefix(m.Groups[1].Value) : 0;
        }
    }
}
