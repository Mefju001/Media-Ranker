using Application.Common.Interfaces;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;

namespace Application.Features.ToWatchServices.Add
{
    public class AddHandler:IRequestHandler<AddCommand, Unit>
    {
        private readonly IMediaRepository<Media> mediaRepository;
        private readonly IUserDetailsRepository userDetailsRepository;
        public AddHandler(IMediaRepository<Media> mediaRepository, IUserDetailsRepository userDetailsRepository)
        {
            this.mediaRepository = mediaRepository;
            this.userDetailsRepository = userDetailsRepository;
        }
        public async Task<Unit> Handle(AddCommand request, CancellationToken cancellationToken)
        {
            var media = await mediaRepository.ExistById(request.MediaId, cancellationToken);
            if(!media)
            {
                throw new NotFoundException($"Media with id {request.MediaId} not found.");
            }
            var user = await userDetailsRepository.GetByIdAsync(request.UserId, cancellationToken)??throw new NotFoundException($"User with id {request.UserId} not found.");
            user.AddToWatch(request.MediaId);
            return Unit.Value;
        }
    }
}
