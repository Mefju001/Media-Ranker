using Application.Features.Auth.Common;
using MediatR;

namespace Application.Features.AdminPanel.ChangePassword
{
    internal class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        private readonly IIdentityService identityService;
        public ChangePasswordHandler(IIdentityService identityService)
        {
            this.identityService = identityService;
        }
        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentException("Password cannot be empty.");
            }
            await identityService.ChangePasswordAdmin(request.userId, request.Password);
            return Unit.Value;
        }
    }
}
