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
                      .SetError(new { Details = "The resource you are trying to access does not exist." });
                    break;

                case UnauthorizedAccessException unauthorizedAccessException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    builder
                      .SetStatus(response.StatusCode)
                      .SetMessage("Authentication is required to access this resource.")
                      .SetError(new { Details = "You need to log in or provide valid credentials to access this resource." });
                    break;

                case ValidationException validationException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    builder
                      .SetStatus(response.StatusCode)
                      .SetMessage("There were validation errors in the provided data.")
                      .SetError(new { Details = validationException.Message });
                    break;


                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    builder
                     .SetStatus(response.StatusCode)
                     .SetMessage("An unexpected error occurred.")
                     .SetError(new { Details = "Please try again later or contact support if the problem persists. " + exception.Message });
                    break;
            }

            var responseDto = builder.Build();
            var json = JsonSerializer.Serialize(responseDto);
            return context.Response.WriteAsync(json);
        }
    }
}
