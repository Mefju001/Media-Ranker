using Domain.Entity;
using System.Security.Claims;

namespace Application.Common.Interfaces
{
    public interface ITokenRepository
    {
        Task<Token?> GetByJtiAsync(string jti, CancellationToken cancellationToken);
        Task SaveToken(Token token, CancellationToken cancellationToken);
        Task<int> DeleteTokensFromUserId(Guid userId, string? jti, CancellationToken cancellationToken);
        Task<List<Token>> GetTokensToCleanUp(CancellationToken cancellationToken);
        Task<int> CleanUpTokensAsync(CancellationToken cancellationToken);
    }
}
