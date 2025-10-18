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
            _logger.LogInformation($"Incoming request: {fullUrl}");

            try
            {
                await _next(context);
                _logger.LogInformation($"Request {fullUrl} executed successfully");
            } catch (Exception)
            {
                _logger.LogWarning($"Request {fullUrl} failed with exception");
                throw;
            }
        }
    }
}
