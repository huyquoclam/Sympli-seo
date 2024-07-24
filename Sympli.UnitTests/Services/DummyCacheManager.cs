using Sympli.Application.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.UnitTests.Services
{
    internal class DummyCacheManager : ICacheManager
    {
        public Task<T> GetOrSetAsync<T>(string key, int cacheTimeInSeconds, Func<Task<T>> acquire)
        {
            return acquire();
        }

        public Task RemoveAsync(string key)
        {
            throw new NotImplementedException();
        }

        public void Set<T>(string key, int cacheTimeInSeconds, T cacheEntry)
        {
            throw new NotImplementedException();
        }
    }
}
