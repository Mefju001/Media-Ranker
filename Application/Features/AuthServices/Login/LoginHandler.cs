using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Features.AuthServices.Common;
using Domain.Entity;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.AuthServices.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, TokenResponse?>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly AccessTokenService accessTokenService;
        private readonly RefreshTokenService refreshTokenService;
        public LoginHandler(IUnitOfWork unitOfWork, IPasswordHasher<User> _passwordHasher, AccessTokenService generateAccessToken, RefreshTokenService refreshTokenService)
        {
            this.unitOfWork = unitOfWork;
            this.passwordHasher = _passwordHasher;
            this.accessTokenService = generateAccessToken;
            this.refreshTokenService = refreshTokenService;
        }

        public async Task<TokenResponse?> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetUserIdByUsername(command.username);
            if (user is not null || (passwordHasher.VerifyHashedPassword(user, user.password, command.password)) == PasswordVerificationResult.Success)
            {
                throw new InvalidCredentialsException("Wrong username or password");
            }
            var accessToken = accessTokenService.generateAccessToken(user.Id, command.username, user.UserRoles.ToList());
            var refreshToken = await refreshTokenService.GenerateRefreshToken(user.Id, command.username);
            return new TokenResponse(accessToken, refreshToken);
        }
    }
}
