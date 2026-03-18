using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;

        public UnitOfWork(AppDbContext context) => this.context = context;

        public async Task<int> CompleteAsync(CancellationToken cancellationToken)
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        {
            return context.Database.BeginTransactionAsync();
        }
    }
}
