using Application.Features.Common.Interfaces;
using Domain.Aggregate;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Database.Repository
{
    public class TokenRepository(IAppDbContext appDbContext) : ITokenRepository
    {
        private readonly DbSet<Token> dbSet = appDbContext.Set<Token>();
        public async Task<int> CleanUpTokensAsync(CancellationToken cancellationToken)
        {
            return await dbSet.Where(x => x.IsRevoked == true || x.ExpiryDate < DateTime.UtcNow).ExecuteDeleteAsync(cancellationToken);
        }

        public async Task<int> DeleteTokensFromUserId(Guid userId, string? jti, CancellationToken cancellationToken)
        {
            var query = dbSet
                .Where(t => t.UserId == userId);
            if (!string.IsNullOrEmpty(jti))
            {
                query = query.Where(t => t.Id == jti);
            }
            return await query.ExecuteDeleteAsync(cancellationToken);
        }
        public async Task SaveToken(Token token, CancellationToken cancellationToken)
        {
            if (token == null) throw new ArgumentNullException();
            await dbSet.AddAsync(token);
        }

        public async Task<Token> GetByJtiAsync(string jti, CancellationToken cancellationToken)
        {
            var result = await dbSet.FirstOrDefaultAsync(t => t.Id == jti && !t.IsRevoked && t.ExpiryDate >= DateTime.UtcNow, cancellationToken);
            return result;
        }
    }
}
