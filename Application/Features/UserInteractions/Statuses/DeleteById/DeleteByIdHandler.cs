using Domain.Exceptions;
using Domain.Repository;
using MediatR;

namespace Application.Features.UserInteractions.Statuses.DeleteById
{
    internal class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand, Unit>
    {
        private readonly IUserDetailsRepository userDetailsRepository;
        public DeleteByIdHandler(IUserDetailsRepository userDetailsRepository)
        {
            this.userDetailsRepository = userDetailsRepository;
        }
        public async Task<Unit> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
        {
            var user = await userDetailsRepository.GetByIdAsync(request.UserId, cancellationToken) ?? throw new NotFoundException($"User with id {request.UserId} not found.");
            user.RemoveInteraction(request.MediaId);
            return Unit.Value;
        }
    }
}
