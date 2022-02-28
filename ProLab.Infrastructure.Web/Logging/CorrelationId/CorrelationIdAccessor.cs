using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace ProLab.Infrastructure.Web.Logging
{

    public class CorrelationIdAccessor : ICorrelationIdAccessor
    {
        private readonly ILogger<CorrelationIdAccessor> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CorrelationIdAccessor(ILogger<CorrelationIdAccessor> logger, IHttpContextAccessor httpContextAccessor)
        {
            this._logger = logger;
            this._httpContextAccessor = httpContextAccessor;
        }

        public string GetCorrelationId()
        {
            try
            {
                var context = this._httpContextAccessor.HttpContext;
                var result = context?.Items["CorrelationId"] as string;

                return result;
            }
            catch (Exception exception)
            {
                this._logger.LogWarning(exception, "Unable to get correlation id in header");
            }

            return string.Empty;
        }
    }
}
