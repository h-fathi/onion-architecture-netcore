using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLab.Infrastructure.Web.Logging.Middlewares
{
    public class CorrelationLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationLoggingMiddleware> _logger;
        public CorrelationLoggingMiddleware(RequestDelegate next, ILogger<CorrelationLoggingMiddleware> logger)
        {
            this._next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = "";
            var header = context.Request.Headers["CorrelationId"];
            if (header.Count > 0)
            {
                correlationId = header[0];
            }
            else
            {
                correlationId = Guid.NewGuid().ToString();
                context.Items["CorrelationId"] = correlationId;
            }

            // Set all the common properties available for every request
            //LogContext.PushProperty("Host", context.Request.Host, destructureObjects: true);
            //LogContext.PushProperty("Protocol", context.Request.Protocol, destructureObjects: true);
            //LogContext.PushProperty("Scheme", context.Request.Scheme, destructureObjects: true);


            // Continue down the Middleware pipeline, eventually returning to this class
            using (_logger.BeginScope("{@CorrelationId}", correlationId))
            {
                await this._next(context);
            }

        }

    }
}
