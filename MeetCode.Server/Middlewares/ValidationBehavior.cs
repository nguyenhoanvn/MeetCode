using Ardalis.Result;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Server.Middlewares
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
            {
                var validationErrors = failures
                    .Select(f => new ValidationError(f.PropertyName, f.ErrorMessage))
                    .ToList();

                if (typeof(TResponse).IsGenericType &&
                typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
                {
                    var resultType = typeof(TResponse).GetGenericArguments()[0];
                    var genericInvalid = typeof(Result<>)
                        .MakeGenericType(resultType)
                        .GetMethod(nameof(Result<object>.Invalid), new[] { typeof(List<ValidationError>) })
                        ?.Invoke(null, new object[] { validationErrors });

                    return (TResponse)genericInvalid!;
                }

                return (TResponse)(object)Result.Invalid(validationErrors);
            }

            return await next();
        }
    }
}
