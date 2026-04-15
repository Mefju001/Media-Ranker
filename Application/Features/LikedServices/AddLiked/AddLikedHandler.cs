using Application.Common.Interfaces;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.LikedServices.AddLiked
{
    public class AddLikedHandler : IRequestHandler<AddLikedCommand, bool>
    {
        private readonly ILogger<AddLikedHandler> logger;
        private readonly IMediaRepository<Media> mediaRepository;
        private readonly IUserDetailsRepository userDetailsRepository;

        public AddLikedHandler(ILogger<AddLikedHandler> logger, IMediaRepository<Media> mediaRepository, IUserDetailsRepository userDetailsRepository)
        {
            this.logger = logger;
            this.mediaRepository = mediaRepository;
            this.userDetailsRepository = userDetailsRepository;
        }

        public async Task<bool> Handle(AddLikedCommand request, CancellationToken cancellationToken)
        {
            var user = await userDetailsRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
            {
                logger.LogWarning("User not found. UserId: {UserId}", request.UserId);
                throw new NotFoundException("User not found.");
            }
            var media = await mediaRepository.ExistById(request.MediaId, cancellationToken);
            if (media is false)
            {
                logger.LogWarning("Media not found. MediaId: {MediaId}", request.MediaId);
                throw new NotFoundException("Media not found.");
            }
            user.AddLikedMedia(request.MediaId);
            return true;
        }
    }
}
