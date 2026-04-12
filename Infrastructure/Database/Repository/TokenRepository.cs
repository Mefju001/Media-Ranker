using Domain.Entity;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Database.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly AppDbContext appDbContext;
        public TokenRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<int> CleanUpTokensAsync(CancellationToken cancellationToken)
        {
            return await appDbContext.Tokens.Where(x => x.IsRevoked == true || x.ExpiryDate < DateTime.UtcNow).ExecuteDeleteAsync(cancellationToken);
        }

        public async Task<int> DeleteTokensFromUserId(Guid userId, string? jti, CancellationToken cancellationToken)
        {
            var query = appDbContext.Tokens
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
            await appDbContext.Tokens.AddAsync(token);
        }

        public async Task<Token> GetByJtiAsync(string jti, CancellationToken cancellationToken)
        {
            var result = await appDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == jti && !t.IsRevoked && t.ExpiryDate >= DateTime.UtcNow, cancellationToken);
            return result;
        }
    }
}
