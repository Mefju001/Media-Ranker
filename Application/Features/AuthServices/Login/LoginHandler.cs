using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Features.AuthServices.Common;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.AuthServices.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, TokenResponse?>
    {
        private readonly IIdentityService identityService;
        private readonly ITokenService tokenServices;
        private readonly ILogger<LoginHandler> logger;
        public LoginHandler(IIdentityService identityService, ITokenService tokenServices, ILogger<LoginHandler> logger)
        {
            this.identityService = identityService;
            this.tokenServices = tokenServices;
            this.logger = logger;
        }

        public async Task<TokenResponse?> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await identityService.AuthenticateAsync(command.username, command.password);
            if (user is null)
            {
                logger.LogWarning("Failed login attempt for username: {Username}", command.username);
                throw new InvalidCredentialsException("Wrong username or password");
            }
            var accessToken = tokenServices.GenerateAccessToken(user.Id, user.Username, user.Roles);
            var refreshToken = await tokenServices.GenerateRefreshToken(user.Id, user.Username, cancellationToken);
            return new TokenResponse(user.Id, user.Username, accessToken, refreshToken);
        }
    }
}
