using Domain.Entity;

namespace Domain.Repository
{
    public interface ITokenRepository
    {
        Task SaveToken(Token token, CancellationToken cancellationToken);
        Task<int> DeleteTokensFromUserId(Guid userId, string? jti, CancellationToken cancellationToken);
        Task<int> CleanUpTokensAsync(CancellationToken cancellationToken);
        Task<Token> GetByJtiAsync(string jti, CancellationToken cancellationToken);
    }
}
