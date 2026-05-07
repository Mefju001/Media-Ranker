using Domain.Exceptions;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Liked.Delete
{
    public class DeleteHandler : IRequestHandler<DeleteCommand, bool>
    {
        private readonly IUserDetailsRepository userDetailsRepository;
        private readonly ILogger<DeleteHandler> logger;

        public DeleteHandler(IUserDetailsRepository userDetailsRepository, ILogger<DeleteHandler> logger)
        {

            this.userDetailsRepository = userDetailsRepository;
            this.logger = logger;
        }

        public async Task<bool> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            var user = await userDetailsRepository.GetByIdAsync(request.userId, cancellationToken);
            if (user == null)
            {
                logger.LogWarning("User with Id: {UserId} not found for deletion of liked media.", request.userId);
                throw new NotFoundException("User not found");
            }
            user.RemoveInteraction(request.mediaId);
            return true;
        }
    }
}
