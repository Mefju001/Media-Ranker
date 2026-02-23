using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Features.AuthServices.Common;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.AuthServices.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, TokenResponse?>
    {
        private readonly IUserRepository userRepository;
        private readonly TokenServices tokenServices;
        public LoginHandler(IUserRepository userRepository, TokenServices refreshTokenService)
        {
            this.userRepository = userRepository;
            this.tokenServices = refreshTokenService;
        }

        public async Task<TokenResponse?> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await userRepository.AuthenticateAsync(command.username,command.password);
            if (user is null) throw new InvalidCredentialsException("Wrong username or password");
            var username = user.username.Value;
            var roles = await userRepository.getUserRoles(username);
            var accessToken = tokenServices.generateAccessToken(user.Id, username, roles);
            var refreshToken = await tokenServices.GenerateRefreshToken(user.Id, username);
            return new TokenResponse(user.Id,username, accessToken,refreshToken);
        }
    }
}
