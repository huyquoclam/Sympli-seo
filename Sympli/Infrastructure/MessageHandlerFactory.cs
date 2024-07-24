using Sympli.Application.CQRS.Messaging;

namespace Sympli.WebAPI.Infrastructure;

public class MessageHandlerFactory : IHandlerFactory
{
    private readonly IServiceProvider _serviceProvider;
    public MessageHandlerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IEnumerable<THandler> GetHandlers<THandler>(Type type) where THandler : IHandler
    {
        var handlerType = GetHandlerType(type);
        var handlers = _serviceProvider.GetServices(handlerType).Cast<THandler>();
        return handlers;
    }

    private static readonly Dictionary<Type, Type> _handlers = new Dictionary<Type, Type>();
    private static Type GetHandlerType(Type type)
    {
        if (!_handlers.ContainsKey(type))
        {
            throw new ArgumentException(string.Format("{0} doesn't exist in handlers", type.FullName));
        }
        return _handlers[type];
    }

    public static void LoadHandlers(string assemblyName, Type baseType, Type genericBaseType)
    {
        var handlerTypes = GettInterfaceTypes(baseType, genericBaseType, assemblyName);
        foreach (var handlerType in handlerTypes)
        {
            var type = handlerType.GetGenericArguments().FirstOrDefault();
            if (type == null)
            {
                throw new ArgumentException(string.Format("{0} is not the handler", handlerType.FullName));
            }

            if (!_handlers.ContainsKey(type))
            {
                _handlers.Add(type, handlerType);
            }
        }
    }

    private static List<Type> GettInterfaceTypes(Type baseType, Type genericBaseType, string assemblyName)
    {
        var assembly = AppDomain.CurrentDomain.Load(assemblyName);
        var types = assembly.GetTypes().ToList();

        types = assembly.GetTypes()
            .SelectMany(type => type.GetInterfaces())
            .Where(type => type != baseType
                && type != genericBaseType
                && type.IsGenericType
                && type.GetGenericTypeDefinition() == genericBaseType)
            .ToList();

        return types;
    }
}