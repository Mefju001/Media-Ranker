using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Features.AuthServices.Common;
using Domain.Exceptions;
using MediatR;
using System.Security.Claims;


namespace Application.Features.AuthServices.RefreshAccessToken
{
    public class RefreshAccessTokenHandler : IRequestHandler<RefreshAccessTokenCommand, TokenResponse?>
    {
        private readonly IIdentityService identityService;
        private readonly ITokenService tokenServices;
        public RefreshAccessTokenHandler(IIdentityService identityService, ITokenService refreshAccessToken)
        {
            this.identityService = identityService;
            this.tokenServices = refreshAccessToken;
        }

        public async Task<TokenResponse?> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
        {
            var claims = await tokenServices.ValidateAndGetPrincipalFromRefreshToken(request.RefreshToken, cancellationToken);

            var userIdClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid token payload.");
            }

            var user = await identityService.GetUserById(userId, cancellationToken)
                       ?? throw new NotFoundException("User no longer exists.");

            var accessToken = tokenServices.GenerateAccessToken(userId, user.Username, user.Roles);
            var newRefreshToken = await tokenServices.GenerateRefreshToken(userId, user.Username, cancellationToken);


            return new TokenResponse(userId, user.Username, accessToken, newRefreshToken);
        }
    }
}
