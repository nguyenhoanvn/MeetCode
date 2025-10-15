using Microsoft.Data.SqlClient;
using StackExchange.Redis;

namespace ReactASP.Server.Middlewares
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
                    error = ex.Message,
                    status
                };

                context.Response.StatusCode = status;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
