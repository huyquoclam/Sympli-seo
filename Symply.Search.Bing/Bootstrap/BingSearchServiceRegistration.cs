using Microsoft.Extensions.DependencyInjection;
using Sympli.Application.Services;
using Sympli.Search.Bing.Services;

namespace Sympli.Search.Google.Bootstrap;

public static class BingSearchServiceRegistration
{
    public static void AddBingSearchServiceConfiguration(this IServiceCollection services)
    {
        services.AddScoped<BingSearchService>();
    }
}
