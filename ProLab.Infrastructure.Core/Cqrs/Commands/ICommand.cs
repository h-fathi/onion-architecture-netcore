using MediatR;

namespace ProLab.Infrastructure.Core
{
    public interface ICommand : ICommand<ApiResponse>
    {
    }

    public interface ICommand<out TResult> : IRequest<TResult> where TResult : ApiResponse
    {
    }
}