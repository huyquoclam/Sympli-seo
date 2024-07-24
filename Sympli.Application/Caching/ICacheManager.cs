using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.Application.Caching;

public interface ICacheManager
{
    Task<T> GetOrSetAsync<T>(string key, int cacheTimeInSeconds, Func<Task<T>> acquire);
    void Set<T>(string key, int cacheTimeInSeconds, T cacheEntry);
    Task RemoveAsync(string key);
}
