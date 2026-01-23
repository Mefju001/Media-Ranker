using Application.Common.Interfaces;
using Domain.Entity;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Application.Features.AuthServices.Signup
{
    public class SignUpHandler : IRequestHandler<SignUpCommand, SignUpResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPasswordHasher<UserDomain> Hasher;
        public SignUpHandler(IUnitOfWork unitOfWork, IPasswordHasher<UserDomain>hasher)
        {
            this.unitOfWork = unitOfWork;
            this.Hasher = hasher;
        }

        public async Task<SignUpResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            bool exists = await unitOfWork.UserRepository.IsAnyUserWithUsernameAndEmailLikeThat(request.username,request.email);
            if (exists)
                //return false;
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
                user.AddRole(role);
            }
            await unitOfWork.CompleteAsync();
            //return true;
        }
    }
}
