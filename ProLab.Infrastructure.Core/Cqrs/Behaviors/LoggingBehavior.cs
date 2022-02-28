﻿using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace ProLab.Infrastructure.Core.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : ApiResponse
        where TRequest : MediatR.IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation("----- Handling command {CommandName} ({@Command})", typeof(TRequest).Name, request);

            var response = await next();

            _logger.LogInformation("----- Handled command {CommandName} - response: {@Response}", typeof(TRequest).Name, response);

            return response;
        }
    }
}
