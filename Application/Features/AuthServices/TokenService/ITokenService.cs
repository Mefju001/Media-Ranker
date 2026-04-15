using System.Security.Claims;

namespace Application.Features.AuthServices.Common
{
    public interface ITokenService
    {
        string GenerateAccessToken(Guid id, string username, IReadOnlyCollection<string> roles);
        Task<string> GenerateRefreshToken(Guid userId, string username, CancellationToken cancellationToken);
        Task<IReadOnlyList<Claim>> ValidateAndGetPrincipalFromRefreshToken(string token, CancellationToken cancellationToken);
    }
}
