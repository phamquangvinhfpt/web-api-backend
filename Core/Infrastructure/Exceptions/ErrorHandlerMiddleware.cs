using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Core.Infrastructure.Exceptions
{
    public class ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = error switch
                {
                    BaseException e => (int)e.StatusCode,
                    _ => StatusCodes.Status500InternalServerError,
                };
                var problemDetails = new ProblemDetails
                {
                    Status = response.StatusCode,
                    Title = error.Message,
                };
                logger.LogError(error.Message);
                var result = JsonSerializer.Serialize(problemDetails);
                await response.WriteAsync(result);
            }
        }
    }
}