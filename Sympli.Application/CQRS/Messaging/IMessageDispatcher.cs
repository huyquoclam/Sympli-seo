namespace Sympli.Application.CQRS.Messaging;

public interface IMessageDispatcher
{
    Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand;
    Task DispatchAsync<TCommand>(IEnumerable<TCommand> commands) where TCommand : ICommand;
    Task<TResult> DispatchAsync<TResult>(IQuery query);
}
