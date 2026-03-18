using Application.Common.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.LikedServices.Delete
{
    public class DeleteLikedHandler : IRequestHandler<DeleteLikedCommand, bool>
    {
        private readonly ILikedMediaRepository likedMediaRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<DeleteLikedHandler> logger;

        public DeleteLikedHandler(IUnitOfWork unitOfWork, ILikedMediaRepository likedMediaRepository, ILogger<DeleteLikedHandler>logger)
        {
            this.unitOfWork = unitOfWork;
            this.likedMediaRepository = likedMediaRepository;
            this.logger = logger;
        }

        public async Task<bool> Handle(DeleteLikedCommand request, CancellationToken cancellationToken)
        {
            var result = await likedMediaRepository.DeleteByLikedMedia(request.userId, request.mediaId, cancellationToken);
            if (!result)
            {
                logger.LogWarning("Liked media with UserId: {UserId} and MediaId: {MediaId} not found for deletion.", request.userId, request.mediaId);
                throw new NotFoundException("Liked media not found");
            }
            await unitOfWork.CompleteAsync(cancellationToken);
            logger.LogInformation("Liked media with UserId: {UserId} and MediaId: {MediaId} successfully deleted.", request.userId, request.mediaId);
            return true;
        }
    }
}
