using Application.Common.Interfaces;
using MediatR;


namespace Application.Features.AuthServices.Signup
{
    public class SignUpHandler : IRequestHandler<SignUpCommand, SignUpResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        public SignUpHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Task<SignUpResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            /*bool exists = await unitOfWork.Users.AnyAsync(u => u.username == userRequest.username || u.email == userRequest.email);
            if (exists)
                return false;
            var user = new User
            {
                username = userRequest.username,
                password = Hasher.HashPassword(null, userRequest.password),
                name = userRequest.name,
                surname = userRequest.surname,
                email = userRequest.email,
            };
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.CompleteAsync();
            var role = await unitOfWork.Roles.FirstOrDefaultAsync(r => r.role == ERole.User);
            if (role is null) return false;
            var UserRoles = new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            };
            await unitOfWork.UsersRoles.AddAsync(UserRoles);
            await unitOfWork.CompleteAsync();

            return true;*/
            throw new NotImplementedException();
        }
    }
}
