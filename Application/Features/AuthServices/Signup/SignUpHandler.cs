using Application.Common.Interfaces;
using Application.Features.AuthServices.Common;
using Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Application.Features.AuthServices.Signup
{
    public class SignUpHandler : IRequestHandler<SignUpCommand, SignUpResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPasswordHasher<UserDomain> Hasher;
        private readonly AccessTokenService accessTokenService;
        private readonly RefreshTokenService refreshTokenService;
        public SignUpHandler(IUnitOfWork unitOfWork, IPasswordHasher<UserDomain> hasher, RefreshTokenService refreshTokenService, AccessTokenService access)
        {
            this.unitOfWork = unitOfWork;
            this.Hasher = hasher;
            this.refreshTokenService = refreshTokenService;
            this.accessTokenService = access;
        }

        public async Task<SignUpResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            bool exists = await unitOfWork.UserRepository.IsAnyUserWithUsernameAndEmailLikeThat(request.username, request.email);
            if (exists)
                throw new Exception("User with that username or email already exists");
            var user = UserDomain.Create(request.username,
                Hasher.HashPassword(null, request.password),
                request.name,
                request.surname,
                request.email
            );
            user = await unitOfWork.UserRepository.AddUser(user);
            var role = await unitOfWork.RoleRepository.GetByNameAsync("User");
            if (role != null)
            {
                user.AddRole(role.Value);
            }
            await unitOfWork.CompleteAsync();
            var accessToken = accessTokenService.generateAccessToken(user.Id, user.username, user.UserRoles.ToList());
            var refreshToken = await refreshTokenService.GenerateRefreshToken(user.Id, user.username);
            return new SignUpResponse(user.username, accessToken, refreshToken);
        }
    }
}
