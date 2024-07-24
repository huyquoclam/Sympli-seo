namespace Sympli.Application.CQRS.Messaging;

public interface IHandlerFactory
{
    IEnumerable<THandler> GetHandlers<THandler>(Type queryType) where THandler : IHandler;
}
