using Application.Common.Interfaces;
using MediatR;
using System.Diagnostics;

namespace Api.Extensions
{
    public class LoggingBehaviour<TRequest, TResponse>(ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var timer = Stopwatch.StartNew();
            logger.LogInformation("Start handling {RequestName}", requestName);
            try
            {
                var response = await next();
                timer.Stop();
                logger.LogInformation("End Handled {RequestName} successfully in {Elapsed}ms", requestName,timer);
                return response;
            }catch(Exception ex)
            {
                timer.Stop();
                logger.LogError(ex, "Error Failed to handle {RequestName} after {Elapsed}ms", requestName,timer);
                throw;
            }
        }
    }
}
