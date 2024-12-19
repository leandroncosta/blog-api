using api.Exceptions;
using api.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace api.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = context.Response;

            var builder = new ResponseDto<object>.Builder()
           .SetSuccess(false)
           .SetTimestamp(DateTime.UtcNow);

            switch (exception)
            {
                case NotFoundException notFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    builder 
                      .SetStatus(response.StatusCode)
                      .SetMessage(notFoundException.Message)
                      .SetError(new { Details = "Resource not found" });
                    break;

                case UnauthorizedAccessException unauthorizedAccessException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    builder
                      .SetStatus(response.StatusCode)
                      .SetMessage("Authentication is required.")
                      .SetError(new { Details = "Unauthorized" });
                    break;

                case ValidationException validationException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    builder
                      .SetStatus(response.StatusCode)
                      .SetMessage("Validation failed")
                      .SetError(new { Details = validationException.Message });
                    break;


                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    builder
                     .SetStatus(response.StatusCode)
                     .SetMessage("An unexpected error occurred.")
                     .SetError(new { Details = "Internal server error: " + exception.Message });
                    break;
            }

            var responseDto = builder.Build();
            var json = JsonSerializer.Serialize(responseDto);
            return context.Response.WriteAsync(json);
        }
    }
}
