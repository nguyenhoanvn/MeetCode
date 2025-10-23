namespace MeetCode.Server.Middlewares
{
    public class RequestLoggingHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingHandler> _logger;

        public RequestLoggingHandler(
            RequestDelegate next,
            ILogger<RequestLoggingHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var fullUrl = $"{context.Request.Method} {context.Request.Path}{context.Request.QueryString}";
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            _logger.LogInformation("Incoming request: {RequestUrl}", fullUrl);

            try
            {
                await _next(context);
                stopwatch.Stop();
                var statusCode = context.Response.StatusCode;
                if (statusCode == 200)
                {
                    _logger.LogInformation(
                        "Request {RequestUrl} executed successfully in {ElapsedMs}ms",
                        fullUrl,
                        stopwatch.ElapsedMilliseconds);
                }
                else if (statusCode >= 400 && statusCode < 500)
                {
                    _logger.LogWarning(
                        "Request {RequestUrl} failed with client error {StatusCode} in {ElapsedMs}ms",
                        fullUrl,
                        statusCode,
                        stopwatch.ElapsedMilliseconds);
                }
                else
                {
                    _logger.LogInformation(
                        "Request {RequestUrl} completed with status {StatusCode} in {ElapsedMs}ms",
                        fullUrl,
                        statusCode,
                        stopwatch.ElapsedMilliseconds);
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex,
                    "Request {RequestUrl} failed with EXCEPTION in {ElapsedMs}ms",
                    fullUrl,
                    stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}
