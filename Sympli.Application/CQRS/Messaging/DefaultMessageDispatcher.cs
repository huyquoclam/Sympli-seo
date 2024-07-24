using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.Application.CQRS.Messaging;

public class DefaultMessageDispatcher : IMessageDispatcher
{
    private readonly IHandlerFactory _handlerFactory;

    public DefaultMessageDispatcher(IHandlerFactory handlerFactory)
    {
        _handlerFactory = handlerFactory;
    }

    public async Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand
    {
        var commandType = command.GetType();
        var handlers = _handlerFactory.GetHandlers<ICommandHandler>(commandType);
        foreach (var handler in handlers)
        {
            await ((dynamic)handler).HandleAsync((dynamic)command);
        }
    }

    public async Task DispatchAsync<TCommand>(IEnumerable<TCommand> commands) where TCommand : ICommand
    {
        foreach (var command in commands)
        {
            await DispatchAsync(command);
        }
    }

    public async Task<TResult> DispatchAsync<TResult>(IQuery query)
    {
        var queryType = query.GetType();
        var handler = _handlerFactory.GetHandlers<IQueryHandler>(queryType).Single();
        return await ((dynamic)handler).HandleAsync((dynamic)query);
    }
}