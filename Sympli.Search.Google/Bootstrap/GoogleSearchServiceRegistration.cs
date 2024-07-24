using Microsoft.Extensions.DependencyInjection;
using Sympli.Application.Services;
using Sympli.Search.Google.Services;

namespace Sympli.Search.Google.Bootstrap;

public static class GoogleSearchServiceRegistration
{
    public static void AddGoogleSearchServiceConfiguration(this IServiceCollection services)
    {
        services.AddScoped<GoogleSearchService>();
    }
}
