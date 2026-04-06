using Application.Common.Interfaces;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.LikedServices.Delete
{
    public class DeleteLikedHandler : IRequestHandler<DeleteLikedCommand, bool>
    {
        private readonly IUserDetailsRepository userDetailsRepository;
        private readonly ILogger<DeleteLikedHandler> logger;

        public DeleteLikedHandler(IUserDetailsRepository userDetailsRepository, ILogger<DeleteLikedHandler> logger)
        {

            this.userDetailsRepository = userDetailsRepository;
            this.logger = logger;
        }

        public async Task<bool> Handle(DeleteLikedCommand request, CancellationToken cancellationToken)
        {
            var user = await userDetailsRepository.GetByIdAsync(request.userId, cancellationToken);
            if (user == null)
            {
                logger.LogWarning("User with Id: {UserId} not found for deletion of liked media.", request.userId);
                throw new NotFoundException("User not found");
            }
            user.RemoveLikedMedia(request.mediaId);
            return true;
        }
    }
}
