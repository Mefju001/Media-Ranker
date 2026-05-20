using Application.Common.Interfaces;
using Application.Features.Common.Interfaces;
using Domain.Aggregate;
using Domain.Entity;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Liked.Add
{
    internal class AddHandler : IRequestHandler<AddCommand, bool>
    {
        private readonly ILogger<AddHandler> logger;
        private readonly IMediaRepository<Media> mediaRepository;
        private readonly IUserDetailsRepository userDetailsRepository;

        public AddHandler(ILogger<AddHandler> logger, IMediaRepository<Media> mediaRepository, IUserDetailsRepository userDetailsRepository)
        {
            this.logger = logger;
            this.mediaRepository = mediaRepository;
            this.userDetailsRepository = userDetailsRepository;
        }

        public async Task<bool> Handle(AddCommand request, CancellationToken cancellationToken)
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
            user.SetRatingVote(request.MediaId, ERatingVote.Liked);
            return true;
        }
    }
}
