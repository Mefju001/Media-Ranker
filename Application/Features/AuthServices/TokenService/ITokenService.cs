using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AuthServices.Common
{
    public interface ITokenService
    {
        string GenerateAccessToken(Guid id, string username, IReadOnlyCollection<string> roles);
        Task<string> GenerateRefreshToken(Guid userId, string username, CancellationToken cancellationToken);
        Task<IReadOnlyList<Claim>> ValidateAndGetPrincipalFromRefreshToken(string token, CancellationToken cancellationToken);
    }
}
