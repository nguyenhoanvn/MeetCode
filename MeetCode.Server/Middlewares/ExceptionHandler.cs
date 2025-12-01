using System.Net;
using MeetCode.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace MeetCode.Server.Middlewares
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(
            RequestDelegate next,
            ILogger<ExceptionHandler> logger
        )
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception at {Path}", context.Request.Path);

                if (ex is FluentValidation.ValidationException validationEx)
                {
                    var errors = validationEx.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );

                    var validationResponse = new
                    {
                        message = "Validation failed.",
                        errors
                    };

                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(validationResponse);
                    return;
                }

                int status;
                string message = ex.Message;

                var exType = ex.GetType();
                if (exType.IsGenericType &&
                    exType.GetGenericTypeDefinition() == typeof(DuplicateEntityException<>))
                {
                    status = StatusCodes.Status409Conflict;
                }
                else if (exType.IsGenericType &&
                    exType.GetGenericTypeDefinition() == typeof(EntityNotFoundException))
                {
                    status = StatusCodes.Status404NotFound;
                }
                else
                {
                    status = ex switch
                    {
                        InvalidOperationException => StatusCodes.Status400BadRequest,
                        BadHttpRequestException => StatusCodes.Status400BadRequest,
                        UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                        DbUpdateException => StatusCodes.Status500InternalServerError,
                        TimeoutException => StatusCodes.Status504GatewayTimeout,
                        RedisConnectionException => StatusCodes.Status503ServiceUnavailable,
                        HttpRequestException => StatusCodes.Status502BadGateway,
                        _ => StatusCodes.Status500InternalServerError
                    };
                }

                var response = new ProblemDetails
                {
                    Title = $"{nameof(ex)}",
                    Status = status,
                    Detail = ex.Message,
                    Instance = context.Request.Path
                };

                context.Response.StatusCode = status;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
