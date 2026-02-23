using Application.Common.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.LikedServices.Delete
{
    public class DeleteLikedHandler : IRequestHandler<DeleteLikedCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILikedMediaRepository likedMediaRepository;

        public DeleteLikedHandler(IUnitOfWork unitOfWork, ILikedMediaRepository mediaRepository)
        {
            this.unitOfWork = unitOfWork;
            this.likedMediaRepository = mediaRepository;
        }

        public async Task<bool> Handle(DeleteLikedCommand request, CancellationToken cancellationToken)
        {
            var result = await likedMediaRepository.DeleteByLikedMedia(request.userId, request.mediaId);
            if (!result)
            {
                throw new NotFoundException("Liked media not found");
            }
            return result;
        }
    }
}
