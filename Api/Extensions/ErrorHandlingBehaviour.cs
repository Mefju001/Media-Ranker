using Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace Api.Extensions
{
    public class ErrorHandlingBehaviour<TRequest, TResponse>(ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (ValidationException ex)
            {
                logger.LogWarning("Błąd walidacji: {Message}", ex.Message);
                throw;
            }
            catch (DomainException ex)
            {
                logger.LogWarning("Błąd biznesowy: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Niekontrolowany błąd w {Request}", typeof(TRequest).Name);
                throw;
            }
        }
    }
}
