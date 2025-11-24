using System.ComponentModel.DataAnnotations;
using WebApplication1.Exceptions;


namespace WebApplication1.Controllers
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
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception occurred");
                context.Response.StatusCode = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                    ValidationException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };
                context.Response.ContentType = "application/json";
                var Response = new
                {
                    error = ex.Message,
                    type = ex.GetType().Name,
                    status = context.Response.StatusCode
                };
                await context.Response.WriteAsJsonAsync(Response);
            }
        }
    }
}
