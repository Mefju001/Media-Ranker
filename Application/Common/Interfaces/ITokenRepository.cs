using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface ITokenRepository
    {
        Task SaveToken(Token token);
        Task<bool> DeleteTokensFromUserId(int userId);
        Task<List<Token>> GetTokensToCleanUp();
        Task RemoveListOfTokens(List<Token> tokens);
    }
}
