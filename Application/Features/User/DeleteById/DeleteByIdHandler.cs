using Application.Features.Common.Interfaces;
using MediatR;


namespace Application.Features.User.DeleteById
{
    public class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand, Unit>
    {
        private readonly IIdentityService identityService;

        public DeleteByIdHandler(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
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
