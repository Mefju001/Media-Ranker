using Application.Common.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.LikedServices.Delete
{
    public class DeleteLikedHandler : IRequestHandler<DeleteLikedCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;

        public DeleteLikedHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteLikedCommand request, CancellationToken cancellationToken)
        {
            var result = await unitOfWork.LikedMediaRepository.DeleteByLikedMedia(request.mediaId, request.userId);
            if (!result)
            {
                throw new NotFoundException("Liked media not found");
            }
            return result;
        }
    }
}
