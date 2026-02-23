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

        public async Task<bool> DeleteTokensFromUserId(Guid userId)
        {
            var userRefreshTokens = await appDbContext.Tokens
                .Where(t => t.UserId.Equals(userId))
                .ToListAsync();
            if (userRefreshTokens.Any())
            {
                appDbContext.Tokens.RemoveRange(userRefreshTokens);
                return true;
            }
            return false;
        }
        public async Task<List<Token>> GetTokensToCleanUp()
        {
            return await appDbContext.Tokens
                .Where(t => t.IsRevoked == true || t.ExpiryDate < DateTime.UtcNow)
                .ToListAsync();
        }
        public async Task RemoveListOfTokens(List<Token> tokens)
        {
            appDbContext.Tokens.RemoveRange(tokens);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<Token?> GetByJtiAsync(Claim jti)
        {
            var result = await appDbContext.Tokens.FirstOrDefaultAsync(t => t.Jti == jti.Value && !t.IsRevoked && t.ExpiryDate >= DateTime.UtcNow);
            return result;
        }
    }
}
