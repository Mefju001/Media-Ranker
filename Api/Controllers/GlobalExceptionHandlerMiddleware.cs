using Domain.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Api.Controllers
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> logger;
        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (OperationCanceledException)
            {
                logger.LogWarning("Request was cancelled by the client.");
                await HandleExceptionAsync(context, "Request was cancelled", "OperationCanceledException", 499);
            }
            catch (Exception ex)
            {
                var statusCode = MapExceptionToStatusCode(ex);
                if (statusCode >= 500)
                {
                    logger.LogError(ex, "Unhandled server exception: {Message}", ex.Message);
                }
                else
                {
                    logger.LogWarning("Client error: {Message} | Type: {Type} | Status: {Status}",
                        ex.Message, ex.GetType().Name, statusCode);
                }

                await HandleExceptionAsync(context, ex.Message, ex.GetType().Name, statusCode);
            }
        }
        private static int MapExceptionToStatusCode(Exception ex)
        {
            return ex switch
            {
                NotFoundException => (StatusCodes.Status404NotFound),
                UserNotFoundException e => (StatusCodes.Status404NotFound),
                EmailAlreadyExistsException e => (StatusCodes.Status400BadRequest),
                InvalidCredentialsException e => (StatusCodes.Status401Unauthorized),
                PasswordMismatchException e => (StatusCodes.Status400BadRequest),
                InvalidRefreshTokenException e => (StatusCodes.Status401Unauthorized),
                NewPasswordIsSameAsOldException e => (StatusCodes.Status400BadRequest),
                UnauthorizedException e => (StatusCodes.Status401Unauthorized),
                BadRequestException e => (StatusCodes.Status400BadRequest),

                ArgumentOutOfRangeException => (StatusCodes.Status400BadRequest),
                ValidationException => (StatusCodes.Status400BadRequest),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized),


                _ => (StatusCodes.Status500InternalServerError)
            };
        }
        private static async Task HandleExceptionAsync(HttpContext httpContext, string message, string name, int statusCode)
        {
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";
            var Response = new
            {
                error = message,
                type = name,
                status = statusCode
            };
            await httpContext.Response.WriteAsJsonAsync(Response);
        }
    }
}
