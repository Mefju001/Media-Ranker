using Application.Common.Interfaces;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastructure.Persistence.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly AppDbContext appDbContext;

        public TokenRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task SaveToken(Token token)
        {
            if (token == null) throw new ArgumentNullException();
            await appDbContext.Tokens.AddAsync(token);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteTokensFromUserId(Guid userId, string? jti)
        {
            var query = appDbContext.Tokens
                .Where(t => t.UserId == userId);
            if(!string.IsNullOrEmpty(jti))
            {
                query = query.Where(t => t.Jti == jti);
            }
            return await query.ExecuteDeleteAsync();
        }
        public async Task<List<Token>> GetTokensToCleanUp()
        {
            return await appDbContext.Tokens
                .Where(t => t.IsRevoked == true || t.ExpiryDate < DateTime.UtcNow)
                .ToListAsync();
        }
        public async Task<int> CleanUpTokensAsync()
        {
            return appDbContext.Tokens.Where(x=>x.IsRevoked == true || x.ExpiryDate < DateTime.UtcNow).ExecuteDelete();
        }

        public async Task<Token?> GetByJtiAsync(string jti)
        {
            var result = await appDbContext.Tokens.FirstOrDefaultAsync(t => t.Jti == jti && !t.IsRevoked && t.ExpiryDate >= DateTime.UtcNow);
            return result;
        }
    }
}
