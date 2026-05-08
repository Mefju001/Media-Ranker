using Domain.Exceptions;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Liked.DeleteById
{
    internal class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand, bool>
    {
        private readonly IUserDetailsRepository userDetailsRepository;
        private readonly ILogger<DeleteByIdHandler> logger;

        public DeleteByIdHandler(IUserDetailsRepository userDetailsRepository, ILogger<DeleteByIdHandler> logger)
        {

            this.userDetailsRepository = userDetailsRepository;
            this.logger = logger;
        }

        public async Task<bool> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
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
