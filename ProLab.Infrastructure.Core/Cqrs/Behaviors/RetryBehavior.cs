using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ProLab.Infrastructure.Core.Behaviors
{
    public class RetryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : MediatR.IRequest<TResponse>
        where TResponse : Result
    {
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            throw new System.NotImplementedException();
        }
    }
}