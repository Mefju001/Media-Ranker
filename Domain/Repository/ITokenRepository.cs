using Domain.Entity;
using System.Security.Claims;

namespace Application.Common.Interfaces
{
    public interface ITokenRepository
    {
        Task<Token?> GetByJtiAsync(Claim jti);
        Task SaveToken(Token token);
        Task<bool> DeleteTokensFromUserId(Guid userId);
        Task<List<Token>> GetTokensToCleanUp();
        Task RemoveListOfTokens(List<Token> tokens);
    }
}
