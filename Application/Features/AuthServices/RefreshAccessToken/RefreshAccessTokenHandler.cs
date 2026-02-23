using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Features.AuthServices.Common;
using Domain.Entity;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Application.Features.AuthServices.RefreshAccessToken
{
    public class RefreshAccessTokenHandler:IRequestHandler<RefreshAccessTokenCommand, TokenResponse?>
    {
        private readonly IUserRepository userRepository;
        private readonly TokenServices tokenServices;
        public RefreshAccessTokenHandler(IUserRepository userRepository, TokenServices refreshAccessToken)
        {
            this.userRepository = userRepository;
            this.tokenServices = refreshAccessToken;
        }

        public async Task<TokenResponse?> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.accessToken))
            {
                throw new NotFoundException("Not found your user");
            }
            
            var claims = await tokenServices.ValidateAndGetPrincipalFromRefreshToken(request.accessToken);
            var stringId = claims.FirstOrDefault(c=>c.Type == JwtRegisteredClaimNames.Sub).Value ?? throw new UserClaimNotFoundException("Not found your id in claims");
            Guid.TryParse(stringId, out var id);
            var username = await userRepository.GetUsernameById(id);
            var roles = await userRepository.getUserRoles(username);
            var accessToken = tokenServices.generateAccessToken(id, username, roles);
            var RefreshToken = await tokenServices.GenerateRefreshToken(id, username);
            return new TokenResponse(id, username, accessToken, RefreshToken);
        }
    }
}
