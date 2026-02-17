using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Features.AuthServices.Common;
using Domain.Entity;
using Domain.Exceptions;
using MediatR;
using System.IdentityModel.Tokens.Jwt;


namespace Application.Features.AuthServices.RefreshAccessToken
{
    public class RefreshAccessTokenHandler:IRequestHandler<RefreshAccessTokenCommand, TokenResponse?>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly AccessTokenService accessTokenService;
        private readonly RefreshTokenService refreshTokenService;
        private readonly ValidatorForRefreshToken validatorForRefreshToken;

        public RefreshAccessTokenHandler(IUnitOfWork unitOfWork, AccessTokenService accessTokenService, RefreshTokenService refreshAccessToken, ValidatorForRefreshToken validatorForRefreshToken)
        {
            this.unitOfWork = unitOfWork;
            this.accessTokenService = accessTokenService;
            this.refreshTokenService = refreshAccessToken;
            this.validatorForRefreshToken = validatorForRefreshToken;
        }

        public async Task<TokenResponse?> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.accessToken))
            {
                throw new NotFoundException("Not found your user");
            }
            var claims = await validatorForRefreshToken.ValidateAndGetPrincipalFromRefreshToken(request.accessToken);
            var username = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name) ?? throw new UserClaimNotFoundException("Not found your user in claims");
            User user = await unitOfWork.UserRepository.GetUserByUsername(username.Value.ToString());
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            var accessToken = accessTokenService.generateAccessToken(user.Id, username.Value.ToString(), user.UserRoles.ToList());
            var RefreshToken = await refreshTokenService.GenerateRefreshToken(user.Id, username.Value.ToString());
            return new TokenResponse(accessToken, RefreshToken);
        }
    }
}
