using Application.Features.Auth.Common;
using MediatR;

namespace Application.Features.AdminPanel.DeleteById
{
    internal class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand, Unit>
    {
        private readonly IIdentityService identityService;
        public DeleteByIdHandler(IIdentityService identityService)
        {
            this.identityService = identityService;
        }
        public async Task<Unit> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
        {

            if (request.userId == Guid.Empty)
            {
                throw new ArgumentException("User ID cannot be empty.");
            }
            await identityService.DeleteUser(request.userId);
            return Unit.Value;
        }
    }
}
