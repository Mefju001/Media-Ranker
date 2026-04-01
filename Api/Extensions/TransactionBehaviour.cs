using Application.Common.Interfaces;
using Infrastructure.Database;
using MediatR;

namespace Api.Extensions
{
    public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : ICommand<TResponse>
    {
        private readonly AppDbContext app;
        public TransactionBehaviour(AppDbContext app)
        {
            this.app = app;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            using var transaction = await app.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var response = await next();
                await app.SaveChangesAsync(cancellationToken);
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
