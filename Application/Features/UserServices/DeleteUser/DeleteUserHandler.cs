using Application.Common.Interfaces;
using MediatR;


namespace Application.Features.UserServices.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IUserRepository userRepository;

        public DeleteUserHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var result = await userRepository.DeleteUser(request.id);
            if (!result.Succeeded)
            {
                var error = result.Errors.FirstOrDefault()?.Description ?? "Unknown error";
                throw new Exception($"Could not delete user: {error}");
            }
            return Unit.Value;
        }
    }
}
