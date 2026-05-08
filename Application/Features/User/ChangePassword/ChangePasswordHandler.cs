using Application.Features.Auth.Common;
using Domain.Exceptions;
using MediatR;



namespace Application.Features.User.ChangePassword
{
    // maybe change password too Value Object in the future
    internal class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        private readonly IIdentityService identityService;

        public ChangePasswordHandler(IIdentityService identityService)
        {
            this.identityService = identityService;
        }
        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.newPassword != request.confirmPassword)
                throw new PasswordMismatchException("The passwords provided are different.");
            await identityService.ChangePassword(request.userId, request.oldPassword, request.newPassword);
            return Unit.Value;
        }
    }
}
