using Application.Common.Interfaces;
using Application.Features.AuthServices.Common;
using Domain.Entity;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Value_Object;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Application.Features.AuthServices.Signup
{
    public class SignUpHandler : IRequestHandler<SignUpCommand, SignUpResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly TokenServices tokenServices;
        public SignUpHandler(IUserRepository userRepository, TokenServices tokenServices)
        {
            this.userRepository = userRepository;
            this.tokenServices = tokenServices;
        }

        public async Task<SignUpResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            bool exists = await userRepository.IsAnyUserWithUsernameAndEmailLikeThat(request.username, request.email);
            if (exists)
                throw new Exception("User with that username or email already exists");
            var user = User.Create(
                new Username(request.username),
                new Password(request.password),
                new Fullname(request.name,
                request.surname),
                new Email(request.email)
            );

            await userRepository.CreateUserWithDefaultRole(user);
            var roles = new List<string> { "User" };
            var accessToken = tokenServices.generateAccessToken(user.Id, user.Username.Value, roles);
            var refreshToken = await tokenServices.GenerateRefreshToken(user.Id, user.Username.Value);
            return new SignUpResponse(user.Username.Value, accessToken, refreshToken);
        }
    }
}
