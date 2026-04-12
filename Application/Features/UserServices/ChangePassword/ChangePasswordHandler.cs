using Application.Common.Interfaces;
using Domain.Exceptions;
using MediatR;



namespace Application.Features.UserServices.ChangePassword
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        private readonly IUserRepository userRepository;

        public ChangePasswordHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.newPassword != request.confirmPassword)
                throw new PasswordMismatchException("The passwords provided are different.");
            var result = await userRepository.ChangePassword(request.userId, request.oldPassword, request.newPassword);
            if (!result.Succeeded)
            {
                var error = result.Errors.FirstOrDefault()?.Description ?? "Operation failed";
                throw new InvalidCredentialsException($"Password change failed: {error}");
            }
            return Unit.Value;
        }
    }
}
