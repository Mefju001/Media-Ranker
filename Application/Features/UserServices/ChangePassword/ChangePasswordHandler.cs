using Application.Common.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;



namespace Application.Features.UserServices.ChangePassword
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<ChangePasswordHandler> logger;

        public ChangePasswordHandler(IUserRepository userRepository, ILogger<ChangePasswordHandler> logger)
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }
        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.newPassword != request.confirmPassword)
                throw new PasswordMismatchException("The passwords provided are different.");
            var result = await userRepository.ChangePassword(request.userId, request.oldPassword, request.newPassword);
            if (!result.Succeeded)
            {
                var error = result.Errors.FirstOrDefault()?.Description ?? "Operation failed";
                logger.LogWarning("Password change failed for user {UserId}: {Error}", request.userId, error);
                throw new InvalidCredentialsException($"Password change failed: {error}");
            }
            logger.LogInformation("Password changed successfully for user {UserId}", request.userId);
            return Unit.Value;
        }
    }
}
