using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Features.AuthServices.Common;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;


namespace Application.Features.AuthServices.RefreshAccessToken
{
    public class RefreshAccessTokenHandler : IRequestHandler<RefreshAccessTokenCommand, TokenResponse?>
    {
        private readonly IUserRepository userRepository;
        private readonly TokenService tokenServices;
        private readonly ILogger<RefreshAccessTokenHandler> logger;
        public RefreshAccessTokenHandler(IUserRepository userRepository, TokenService refreshAccessToken, ILogger<RefreshAccessTokenHandler> logger)
        {
            this.userRepository = userRepository;
            this.tokenServices = refreshAccessToken;
            this.logger = logger;
        }

        public async Task<TokenResponse?> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                logger.LogWarning("Próba odświeżenia tokena bez podania RefreshToken.");
                throw new UnauthorizedException("Refresh token is required.");
            }
            var claims = await tokenServices.ValidateAndGetPrincipalFromRefreshToken(request.RefreshToken, cancellationToken);
            if (claims == null)
            {
                logger.LogWarning("Nieudana walidacja RefreshToken. Możliwa próba ataku lub wygasła sesja.");
                throw new UnauthorizedException("Token is invalid or has expired.");
            }
            var userIdClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                throw new UnauthorizedException("Invalid token payload.");
            var user = await userRepository.GetUserById(userId, cancellationToken);
            if (user == null)
            {
                logger.LogWarning("Użytkownik o ID {UserId} nie istnieje, mimo poprawnego tokena.", userId);
                throw new NotFoundException("User no longer exists.");
            }
            var accessToken = tokenServices.generateAccessToken(userId, user.Username.Value, user.UserRoles);
            var newRefreshToken = await tokenServices.GenerateRefreshToken(userId, user.Username.Value, cancellationToken);
            logger.LogInformation("Pomyślnie odświeżono token dla użytkownika: {UserId}", userId);
            return new TokenResponse(userId, user.Username.Value, accessToken, newRefreshToken);
        }
    }
}
