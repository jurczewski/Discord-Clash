using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace DiscordClash.API.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case ValidationException e:
                        // validation failed
                        response.StatusCode = (int)HttpStatusCode.Conflict;
                        _logger.LogWarning(e, $"{e.GetType().Name}: {e.Message}");
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        _logger.LogError(e, $"{e.GetType().Name}: {e.Message}");
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        _logger.LogCritical(error, $"Unhandled error: {error.Message}");
                        break;
                }

                var msg = response.StatusCode == (int)HttpStatusCode.InternalServerError ? "Unhandled error" : error.Message;
                var result = JsonSerializer.Serialize(new { message = msg });
                await response.WriteAsync(result);
            }
        }
    }
}
