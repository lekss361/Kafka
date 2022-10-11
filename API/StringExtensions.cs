using System.Collections.Concurrent;

namespace Ekassir.KafkaFlow.Extensions
{
    public static class StringExtensions
    {
        private static readonly ConcurrentDictionary<WildcardOptions, ConcurrentDictionary<string, WildcardPattern>> WildcardCacheByOpts = new();

        public static bool IsWildcardMatch(this string self, string pattern, WildcardOptions options = WildcardOptions.None)
        {
            var cache = WildcardCacheByOpts.GetOrAdd(options,
                (x) => new ConcurrentDictionary<string, WildcardPattern>(StringComparer.Ordinal));
            if (cache.TryGetValue(pattern, out var wildcard))
            {
                return wildcard.IsMatch(self);
            }

            wildcard = new WildcardPattern(pattern, options);
            var result = wildcard.IsMatch(self);
            cache.TryAdd(pattern, wildcard);

            return result;
        }
        
        public static bool IsPresent(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static bool IsMissing(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}