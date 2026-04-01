using Application.Common.Interfaces;
using MediatR;

namespace Api.Extensions
{
    public class LoggingBehaviour<TRequest, TResponse>(ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            logger.LogInformation("Start handling {RequestName}", requestName);
            try
            {
                var response = await next();
                logger.LogInformation("End Handled {RequestName} successfully", requestName);
                return response;
            }catch(Exception ex)
            {
                logger.LogError(ex, "Error Failed to handle {RequestName}", requestName);
                throw;
            }
        }
    }
}
