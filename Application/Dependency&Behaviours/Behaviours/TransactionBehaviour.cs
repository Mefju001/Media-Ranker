using Application.Features.Common.Interfaces;
using MediatR;

namespace Application.Behaviours
{
    public class TransactionBehaviour<TRequest, TResponse>(IAppDbContext app) : IPipelineBehavior<TRequest, TResponse>
            where TRequest : ICommand<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (app.Context.Database.CurrentTransaction != null)
            {
                return await next();
            }
            using var transaction = await app.Context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var response = await next();
                await app.Context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return response;

            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;

            }
        }
    }
}
