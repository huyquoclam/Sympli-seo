using Microsoft.Extensions.DependencyInjection;
using Sympli.Application.Common;
using Sympli.Application.Services;
using Sympli.Search.Bing.Services;
using Sympli.Search.Google.Services;

namespace Sympli.WebAPI.Infrastructure;

public class KeywordSearchServiceFactory(IServiceScopeFactory serviceScopeFactory) : IKeywordSearchServiceFactory
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    public ISearchEngineService GetService(string serviceName)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;

            return serviceName switch
            {
                SearchEngines.Bing => serviceProvider.GetRequiredService<BingSearchService>(),
                SearchEngines.Google => serviceProvider.GetRequiredService<GoogleSearchService>(),
                // TODO: Add more search service
                _ => throw new ArgumentException("Invalid service name", nameof(serviceName))
            };
        }
    }
}
