using Application.Common.Interfaces;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;

namespace Application.Features.Watching.Add
{
    internal class AddHandler:IRequestHandler<AddCommand, bool>
    {
        private readonly IUserDetailsRepository userDetailsRepository;
        private readonly IMediaRepository<Media> mediaRepository;
        public AddHandler(IUserDetailsRepository userDetailsRepository, IMediaRepository<Media> mediaRepository)
        {
            this.userDetailsRepository = userDetailsRepository;
            this.mediaRepository = mediaRepository;
        }
        public async Task<bool> Handle(AddCommand request, CancellationToken cancellationToken)
        {
            var media = await mediaRepository.ExistById(request.mediaId, cancellationToken);
            if(!media)
                throw new NotFoundException($"Media not found");
            var user = await userDetailsRepository.GetByIdAsync(request.userId, cancellationToken);
            if (user is null)
                throw new NotFoundException($"User not found");
            user.SetTypeInteractions(request.mediaId, ETypeInteractions.WATCHING);
            return true;

        }
    }
}
