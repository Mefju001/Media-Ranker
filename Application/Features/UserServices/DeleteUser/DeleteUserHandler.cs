using Application.Common.Interfaces;
using MediatR;


namespace Application.Features.UserServices.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IIdentityService identityService;

        public DeleteUserHandler(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if(request.id == Guid.Empty)
            {
                throw new ArgumentException("User ID cannot be empty.");
            }
            await identityService.DeleteUser(request.id);
            return Unit.Value;
        }
    }
}
