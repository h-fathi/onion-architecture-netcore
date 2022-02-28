using System.Linq;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using ProLab.Infrastructure.Core.ExceptionHandling;

namespace ProLab.Infrastructure.Core.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : MediatR.IRequest<TResponse>
        where TResponse : ApiResponse
    {
        private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly IDomainExceptionManager _domainExceptionManager;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators,
            ILogger<ValidationBehavior<TRequest, TResponse>> logger,
            IDomainExceptionManager domainExceptionManager)
        {
            _validators = validators;
            _logger = logger;
            _domainExceptionManager = domainExceptionManager;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var typeName = typeof(TRequest).Name;

            _logger.LogInformation("----- Validating command {CommandType}", typeName);

            var failures = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (failures.Any())
            {
                _logger.LogInformation(
                    "Validation Failures - {CommandType} - Command: {@Command} - Errors: {@Failures}", typeName,
                    request, failures);

                //get error from domain exception manager
                var validationFailure = new List<ErrorMessage>();
                failures.ForEach(error =>
                {

                    if (!string.IsNullOrEmpty(error.ErrorCode))
                    {
                        var errorMessage = _domainExceptionManager.GetByCode(error.ErrorCode);
                        errorMessage.MemberName = error.PropertyName;
                        if (errorMessage != null) validationFailure.Add(errorMessage);
                    }
                    else
                    {
                        validationFailure.Add(new ErrorMessage
                        {
                            MemberName = error.PropertyName,
                            ErrorCode = error.ErrorCode,
                            Message = error.ErrorMessage,
                            FaMessage = error.ErrorMessage
                        });
                    }
                });

                var response = new ApiResponse
                {
                    Message = validationFailure.FirstOrDefault().FaMessage,
                    Errors = validationFailure,
                    Success = false
                };

                return Task.FromResult(response as TResponse);
                //throw new ValidationException($"Command Validation Exception for type {typeof(TRequest).Name}",
                //    failures);
            }

            return next();
        }
    }
}