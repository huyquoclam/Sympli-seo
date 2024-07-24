using Microsoft.Extensions.Options;
using Sympli.Application.Common;
using Sympli.Search.Google.Bootstrap;
using Sympli.Caching.Bootstrap;
using Sympli.Application.Services;
using Sympli.WebAPI.Infrastructure;
using Sympli.Search.Google.Services;
using Sympli.Search.Bing.Services;
using Sympli.Application.CQRS.Messaging;

namespace Sympli.WebAPI.Bootstrap;

public static class Bootstrap
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        // Register application services
        services.RegisterServices();

        // Register application options
        services.RegisterApplicationSettings<ApplicationOptions>(configuration, "ApplicationOptions");

        // Register caching
        services.AddMemoryCaching();

        // Register search services
        services.AddSingleton<IKeywordSearchServiceFactory, KeywordSearchServiceFactory>();

        // Register message dispatcher
        services.RegisterMessagingHandlers();

    }

    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddGoogleSearchServiceConfiguration();
        services.AddBingSearchServiceConfiguration();
    }

    private static void RegisterApplicationSettings<T>(
        this IServiceCollection services, 
        IConfiguration configuration, 
        string settingKey) where T : class
    {
        services.Configure<T>(config =>
        {
            configuration.GetSection(settingKey).Bind(config);
        });
        services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<T>>().Value);
    }

    private static void RegisterMessagingHandlers(this IServiceCollection services)
    {
        services.AddScoped<IMessageDispatcher, DefaultMessageDispatcher>();
        services.AddScoped<IHandlerFactory, MessageHandlerFactory>();
        MessageHandlerFactory.LoadHandlers("Sympli.Application", typeof(IQueryHandler), typeof(IQueryHandler<,>));
        RegisterComponentsByPattern("Sympli.Application", "QueryHandler",
            [typeof(IQueryHandler)],
            (serviceType, implementationType) => services.AddTransient(serviceType, implementationType));
    }

    private static void RegisterComponentsByPattern(string assemblyName, string suffixName,
                List<Type> exculudedInterfaceTypes, Action<Type, Type> registerAction)
    {
        var assembly = AppDomain.CurrentDomain.Load(assemblyName);
        var types = assembly.GetTypes().ToList();

        var classTypes = types.Where(type => type.IsClass && !type.IsAbstract
            && type.FullName.StartsWith(assemblyName)
            && type.Name.EndsWith(suffixName))
            .ToList();

        foreach (var classType in classTypes)
        {
            var interfaceTypes = classType.GetInterfaces()
                .Where(p => !exculudedInterfaceTypes.Any(e => e.Name == p.Name))
                .ToList();
            if (interfaceTypes.Count > 0)
            {
                foreach (var interfaceType in interfaceTypes)
                {
                    registerAction(interfaceType, classType);
                }
            }
            else
            {
                registerAction(classType, classType);
            }
        }
    }
}
