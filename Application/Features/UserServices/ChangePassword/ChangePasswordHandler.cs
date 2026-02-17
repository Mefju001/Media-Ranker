using Application.Common.Interfaces;
using Domain.Entity;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;



namespace Application.Features.UserServices.ChangePassword
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPasswordHasher<User> Hasher;

        public ChangePasswordHandler(IPasswordHasher<User> passwordHasher, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            Hasher = passwordHasher;
        }
        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.newPassword) ||
                string.IsNullOrWhiteSpace(request.confirmPassword) ||
                string.IsNullOrWhiteSpace(request.oldPassword))
            {
                throw new ArgumentException("you should fill in these fields with passwords");
            }
            if (string.Equals(request.oldPassword, request.newPassword, StringComparison.Ordinal))
                throw new NewPasswordIsSameAsOldException("The new password is too similar to the old one");
            if (!string.Equals(request.newPassword, request.confirmPassword, StringComparison.Ordinal))
            {
                throw new PasswordMismatchException("The new password differed from the confirmation password");
            }
            var user = await unitOfWork.UserRepository.GetUserById(request.userId);
            if (user is null)
            {
                throw new InvalidCredentialsException("Invalid user or password.");
            }
            var passwordVerificationResult = Hasher.VerifyHashedPassword(user, user.password, request.oldPassword);
            if (passwordVerificationResult is not PasswordVerificationResult.Success)
                throw new InvalidCredentialsException("You write wrong old password");
            user.ChangePassword(Hasher.HashPassword(user, request.newPassword));
            await unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
