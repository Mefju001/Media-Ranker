using Domain.Entity;
using System.Security.Claims;

namespace Application.Common.Interfaces
{
    public interface ITokenRepository
    {
        Task<Token?> GetByJtiAsync(string jti);
        Task SaveToken(Token token);
        Task<int> DeleteTokensFromUserId(Guid userId, string? jti);
        Task<List<Token>> GetTokensToCleanUp();
        Task<int> CleanUpTokensAsync();
    }
}
