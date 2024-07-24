namespace Sympli.Application.CQRS.Messaging;

public interface IQueryHandler : IHandler { }

public interface IQueryHandler<TQuery, TView> : IQueryHandler
    where TQuery : IQuery
    where TView : IView
{
    Task<TView> HandleAsync(TQuery query);
}