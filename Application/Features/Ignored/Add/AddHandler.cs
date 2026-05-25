using Application.Common.Interfaces;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;

namespace Application.Features.Ignored.Add
{
    internal class AddHandler : IRequestHandler<AddCommand, bool>
    {
        private readonly IMediaRepository<Media> mediaRepository;
        private readonly IUserDetailsRepository userDetailsRepository;
        public AddHandler(IMediaRepository<Media>mediaRepository, IUserDetailsRepository userDetailsRepository)
        {
            this.userDetailsRepository = userDetailsRepository;
            this.mediaRepository = mediaRepository;
        }
        public async Task<bool> Handle(AddCommand request, CancellationToken cancellationToken)
        {
            var media = await mediaRepository.ExistById(request.mediaId, cancellationToken);
            if(media == false)
            {
                throw new NotFoundException("Media not found.");
            }
            var user = await userDetailsRepository.GetByIdAsync(request.userId, cancellationToken);
            if (user == null) {
                throw new NotFoundException("User not found.");
            }
            user.SetTypeInteractions(request.mediaId,ETypeInteractions.IGNORED);
            return true;
        }
    }
}
