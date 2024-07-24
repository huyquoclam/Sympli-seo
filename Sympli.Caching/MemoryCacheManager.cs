using Microsoft.Extensions.Caching.Memory;
using Sympli.Application.Caching;

namespace Sympli.Caching;

public class MemoryCacheManager(IMemoryCache cache) : ICacheManager
{
    private readonly IMemoryCache _cache = cache;

    /// <summary>
    /// Get data from cache or set it if it's not there
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="cacheTimeInSeconds"></param>
    /// <param name="acquire"></param>
    /// <returns></returns>
    public async Task<T> GetOrSetAsync<T>(string key, int cacheTimeInSeconds, Func<Task<T>> acquire)
    {
        if (cacheTimeInSeconds == 0) // to disable caching, set cacheTimeInSeconds to 0
            return await acquire();

        // Look for cache key.
        if (!_cache.TryGetValue(key, out T cacheEntry))
        {
            // Key not in cache, so get data.
            cacheEntry = await acquire();

            // Set cache options.
            Set(key, cacheTimeInSeconds, cacheEntry);
        }
        return cacheEntry;
    }

    /// <summary>
    /// Set cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="cacheTimeInSeconds"></param>
    /// <param name="cacheEntry"></param>
    public void Set<T>(string key, int cacheTimeInSeconds, T cacheEntry)
    {
        // Set cache options.
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            // Keep in cache for this time, reset cache in every {cacheTimeInSeconds} 
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheTimeInSeconds));

        // Save data in cache.
        _cache.Set(key, cacheEntry, cacheEntryOptions);
    }

    /// <summary>
    /// Remove cache by key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Task RemoveAsync(string key)
    {
        _cache.Remove(key);

        return Task.CompletedTask;
    }
}
