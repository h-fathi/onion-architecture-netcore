using MediatR;

namespace ProLab.Infrastructure.Core
{
    public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, ApiResponse> where TCommand : ICommand
    {
    }

    public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult>
        where TCommand : ICommand<TResult> where TResult : ApiResponse
    {
    }
}