using Sympli.Application.Caching;
using Sympli.Application.Common;
using Sympli.Application.Services;
using System.Net.Http;
using System.Text.RegularExpressions;
using static Sympli.Application.Caching.CacheExtensions;

namespace Sympli.Search.Bing.Services;

public class BingSearchService(
    ICacheManager cacheManager,
    ApplicationOptions applicationOptions) 
    : BaseSearchEngineService(), ISearchEngineService
{
    private readonly ApplicationOptions _applicationOptions = applicationOptions;
    private readonly ICacheManager _cacheManager = cacheManager;
    public override string DefaultSympliKeywords => "e-settlements";
    public override string DefaultSympliUrl => "sympli.com.au";
    protected override int MaxSearchResults => _applicationOptions.MaxSearchResults;
    protected override string DefaultSearchPattern => @"<cite[^>]*>(.*?)((https?:\/\/)?(www\.)?[-a-zA-Z0-9@:%._\+~#=]+(\.[a-zA-Z0-9()]{1,6})(.*?))(.*?)<\/cite>";
    protected override string SearchEngineUrl => $"https://www.bing.com/search?q=";
    protected override string OffsetParams => "first";
    protected override string CountParams => "count";

    public async Task<List<int>> GetUrlPositionsResultsWithCustomKeywordsAndUrl(string keywords, string url)
    {
       return await _cacheManager.GetOrSetAsync(
                    CacheExtensions.FormatCacheKey(
                        CacheKeys.BingEngineSearch, 
                        keywords,
                        url), 
                    _applicationOptions.EngineSearchCachingInSeconds,
                    async () => await base.GetUrlPositionsResults(keywords, url));
    }

    public async Task<List<int>> GetUrlPositionsResults()
    {
        return await GetUrlPositionsResultsWithCustomKeywordsAndUrl(DefaultSympliKeywords, DefaultSympliUrl);
    }
}
