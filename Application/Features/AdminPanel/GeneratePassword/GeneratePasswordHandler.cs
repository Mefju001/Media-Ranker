using Application.Features.Auth.Common;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Security.Cryptography;

namespace Application.Features.AdminPanel.GeneratePassword
{

    internal class GeneratePasswordHandler : IRequestHandler<GeneratePasswordCommand, Unit>
    {
        private readonly IIdentityService identityService;
        private readonly IEmailSender emailSender;
        public GeneratePasswordHandler(IIdentityService identityService, IEmailSender emailSender)
        {
            this.identityService = identityService;
            this.emailSender = emailSender;
        }
        public async Task<Unit> Handle(GeneratePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await identityService.GetUserById(request.userId, cancellationToken);
            if(user is null)
            {
                throw new NotFoundException("User not found");
            }
            var password = GenerateRandomPassword(12);
            await identityService.ChangePasswordAdmin(request.userId, password);
            await emailSender.SendEmailAsync(user.Email, "Password Reset", $"Your new password is: {password}");
            return Unit.Value;
        }
        private string GenerateRandomPassword(int length)
        {
            const string Small = "abcdefghijklmnopqrstuvwxyz";
            const string Large = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string Digits = "0123456789";
            const string SpecialChars = "!@#$%^&*()";

            const string ValidChars = Small + Large + Digits + SpecialChars;
            string randomStr = string.Empty;
            do
            {
                randomStr = RandomNumberGenerator.GetString(ValidChars, length);
            } while (!randomStr.Any(char.IsLower) || !randomStr.Any(char.IsUpper) ||
                    !randomStr.Any(char.IsDigit) || !randomStr.Any(c => SpecialChars.Contains(c)));
            return randomStr;
        }
    }
}
