using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MeetCode.Server.Middlewares
{
    public class ResultBehaviorHandler<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    {
        private readonly ILogger<ResultBehaviorHandler<TRequest, TResponse>> _logger;

        public ResultBehaviorHandler(ILogger<ResultBehaviorHandler<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct)
        {
            var response = await next();

            if (response is Ardalis.Result.IResult result && (!result.IsOk() && !result.IsCreated()))
            {
                _logger.LogError(
                    "Handler {HandlerName} failed: {Errors}",
                    typeof(TRequest).Name,
                    string.Join(", ", result.Errors)
                );
            }

            return response;
        }
    }
}
