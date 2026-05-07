using Domain.Exceptions;
using Domain.Repository;
using MediatR;

namespace Application.Features.ToWatch.Remove
{
    public class RemoveHandler : IRequestHandler<RemoveCommand, Unit>
    {
        private readonly IUserDetailsRepository userDetailsRepository;
        public RemoveHandler(IUserDetailsRepository userDetailsRepository)
        {
            this.userDetailsRepository = userDetailsRepository;
        }
        public async Task<Unit> Handle(RemoveCommand request, CancellationToken cancellationToken)
        {
            var user = await userDetailsRepository.GetByIdAsync(request.UserId, cancellationToken)??throw new NotFoundException($"User with id {request.UserId} not found.");
            user.RemoveInteraction(request.MovieId);
            return Unit.Value;
        }
    }
}
