namespace Sympli.Application.Caching;

public static class CacheExtensions
{
    public struct CacheKeys
    {
        public const string GoogleEngineSearch= "GoogleEngineSearch";
        public const string BingEngineSearch = "BingEngineSearch";
        public const string YahooEngineSearch = "YahooEngineSearch";
    }

    public static string FormatCacheKey(string key, params object[] args)
    {
        if (args == null || args.Length == 0)
            return key;

        string postFix = string.Join('_', args);
        string internalKey = string.Concat(key, "_", postFix);
        return string.Format(internalKey, args);
    }
}
