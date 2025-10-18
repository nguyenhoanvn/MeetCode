using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MeetCode.Server.Middlewares
{
    public class ResultBehaviorHandler<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
        where TResponse : Result
    {
        private readonly ILogger<ResultBehaviorHandler<TRequest, TResponse>> _logger;

        public ResultBehaviorHandler(ILogger<ResultBehaviorHandler<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            var response = await next();

            if (response is Result result && !result.IsSuccess)
            {
                var errorMessage = string.Join("; ", result.Errors);
                _logger.LogWarning("Command {CommandName} failed: {Errors}", typeof(TRequest).Name, errorMessage);
                throw new BadHttpRequestException(errorMessage);
            }

            return response;
        }
    }
}
