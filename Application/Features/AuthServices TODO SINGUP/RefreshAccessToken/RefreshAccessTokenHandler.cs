using Application.Features.AuthServices.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Domain.Exceptions;
using WebApplication1.Services.Interfaces;

namespace Application.Features.AuthServices.RefreshAccessToken
{
    public class RefreshAccessTokenHandler
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

        public async Task<TokenResponse?> RefreshAccessToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new NotFoundException("Not found your user");
            }
            var claims = await validatorForRefreshToken.ValidateAndGetPrincipalFromRefreshToken(refreshToken);
            var username = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name) ?? throw new UserClaimNotFoundException("Not found your user in claims");
            var user = await unitOfWork.Users
                .FirstOrDefaultAsync(u => u.username == username.Value.ToString());
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            var accessToken = accessTokenService.generateAccessToken(user.Id, username.Value.ToString(), user.UserRoles);
            var RefreshToken = await refreshTokenService.GenerateRefreshToken(user.Id, username.Value.ToString());
            return new TokenResponse(accessToken, RefreshToken);
        }
    }
}
