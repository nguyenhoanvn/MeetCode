using Microsoft.Data.SqlClient;
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
                var status = ex switch
                {
                    InvalidOperationException => StatusCodes.Status400BadRequest,
                    BadHttpRequestException => StatusCodes.Status400BadRequest,
                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                    SqlException => StatusCodes.Status500InternalServerError,
                    TimeoutException => StatusCodes.Status504GatewayTimeout,
                    RedisConnectionException => StatusCodes.Status503ServiceUnavailable,
                    HttpRequestException => StatusCodes.Status502BadGateway,
                    _ => StatusCodes.Status500InternalServerError
                };

                var response = new
                {
                    message = ex.Message,
                    source = ex.Source,
                    status
                };

                context.Response.StatusCode = status;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
