using Application.Common.Interfaces;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly AppDbContext appDbContext;

        public TokenRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task SaveToken(TokenDomain token)
        {
            if (token == null) throw new ArgumentNullException();
            await appDbContext.Tokens.AddAsync(token);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteTokensFromUserId(int userId)
        {
            var userRefreshTokens = await appDbContext.Tokens
                .Where(t => t.UserId == userId)
                .ToListAsync();
            if (userRefreshTokens.Any())
            {
                appDbContext.Tokens.RemoveRange(userRefreshTokens);
                return true;
            }
            return false;
        }
    }
}
