using Domain.Exceptions;
using Domain.Repository;
using MediatR;

namespace Application.Features.UserInteractions.Statuses.SetStatus
{
    internal class SetStatusHandler : IRequestHandler<SetStatusCommand, Unit>
    {
        private readonly IMediaRepository<Media> mediaRepository;
        private readonly IUserDetailsRepository userDetailsRepository;
        public SetStatusHandler(IMediaRepository<Media> mediaRepository, IUserDetailsRepository userDetailsRepository)
        {
            this.mediaRepository = mediaRepository;
            this.userDetailsRepository = userDetailsRepository;
        }
        public async Task<Unit> Handle(SetStatusCommand request, CancellationToken cancellationToken)
        {
            var media = await mediaRepository.ExistById(request.mediaId, cancellationToken);
            if (!media)
            {
                throw new NotFoundException($"Media with id {request.mediaId} not found.");
            }
            var user = await userDetailsRepository.GetByIdAsync(request.userId, cancellationToken) ?? throw new NotFoundException($"User with id {request.userId} not found.");
            if(request.ratingVote.HasValue)
            {
                user.SetRatingVote(request.mediaId, request.ratingVote.Value);
            }
            if(request.typeInteractions.HasValue)
            {
                user.SetTypeInteractions(request.mediaId, request.typeInteractions.Value);
            }
            return Unit.Value;
        }
    }
}
