using Microsoft.Extensions.DependencyInjection;
using Sympli.Application.Caching;

namespace Sympli.Caching.Bootstrap;

public static class MemoryCachingRegistration
{
    public static void AddMemoryCaching(this IServiceCollection services)
    {
        services.AddSingleton<ICacheManager, MemoryCacheManager>();
    }
}
