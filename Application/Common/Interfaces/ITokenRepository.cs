using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface ITokenRepository
    {
        Task SaveToken(TokenDomain token);
        Task<bool> DeleteTokensFromUserId(int userId);
        Task<List<TokenDomain>> GetTokensToCleanUp();
        Task RemoveListOfTokens(List<TokenDomain> tokens);
    }
}
