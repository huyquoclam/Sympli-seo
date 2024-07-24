namespace Sympli.Application.CQRS.Messaging;

public interface ICommandHandler : IHandler { }

public interface ICommandHandler<T> : ICommandHandler
    where T : ICommand
{
    Task HandleAsync(T command);
}
