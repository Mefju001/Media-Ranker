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
        private readonly IUserRepository userRepository;
        private readonly TokenService tokenServices;
        private readonly ILogger<LoginHandler> logger;
        public LoginHandler(IUserRepository userRepository, TokenService tokenServices, ILogger<LoginHandler> logger)
        {
            this.userRepository = userRepository;
            this.tokenServices = tokenServices;
            this.logger = logger;
        }

        public async Task<TokenResponse?> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await userRepository.AuthenticateAsync(command.username, command.password);
            if (user is null)
            {
                logger.LogWarning("Failed login attempt for username: {Username}", command.username);
                throw new InvalidCredentialsException("Wrong username or password");
            }
            var username = user.Username.Value;
            var accessToken = tokenServices.generateAccessToken(user.Id, username, user.UserRoles);
            var refreshToken = await tokenServices.GenerateRefreshToken(user.Id, username, cancellationToken);
            logger.LogInformation("User {Username} logged in successfully", username);
            return new TokenResponse(user.Id, username, accessToken, refreshToken);
        }
    }
}
