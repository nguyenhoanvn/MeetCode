using Ardalis.Result;
using MediatR;

namespace MeetCode.Server.Middlewares
{
    public class HandlerResultLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<HandlerResultLoggingBehavior<TRequest, TResponse>> _logger;

        public HandlerResultLoggingBehavior(ILogger<HandlerResultLoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            var response = await next();

            var responseType = response.GetType();

            if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
            {
                object value = responseType.GetProperty("Value")?.GetValue(response);
                _logger.LogInformation("Handler {Handler} returned with result {@Value}", typeof(TRequest).Name, value);
            }
            else
            {
                _logger.LogInformation("Handler {Handler} returned {@Response}", typeof(TRequest).Name, response);
            }

            return response;
        }
    }
}
