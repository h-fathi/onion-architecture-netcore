using MediatR;

namespace ProLab.Infrastructure.Core
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}
