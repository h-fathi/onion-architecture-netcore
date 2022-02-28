using MediatR;

namespace ProLab.Infrastructure.Core
{
    public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
    }
}