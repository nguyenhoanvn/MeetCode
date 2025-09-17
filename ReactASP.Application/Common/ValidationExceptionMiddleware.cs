using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReactASP.Application.Common
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (FluentValidation.ValidationException ex)
            {
                var modelState = new ModelStateDictionary();

                foreach (var error in ex.Errors)
                {
                    modelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                var problemDetails = new ValidationProblemDetails(modelState)
                {
                    Title = "Validation Error",
                    Status = StatusCodes.Status400BadRequest
                };

                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                httpContext.Response.ContentType = "application/problem+json";

                var jsonResponse = JsonSerializer.Serialize(problemDetails);
                await httpContext.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
