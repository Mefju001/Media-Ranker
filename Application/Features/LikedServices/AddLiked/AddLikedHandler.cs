using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using MediatR;

namespace Application.Features.LikedServices.AddLiked
{
    public class AddLikedHandler : IRequestHandler<AddLikedCommand, LikedMediaResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public AddLikedHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<LikedMediaResponse> Handle(AddLikedCommand request, CancellationToken cancellationToken)
        {
            var media = unitOfWork.MediaRepository.GetMediaById(request.MediaId);
            var user = unitOfWork.UserRepository.GetUserById(request.UserId);
            if (media is null || user is null)
                throw new Exception("is empty");
            var existingLikedMedia = await unitOfWork.LikedMediaRepository.Any(request.UserId, request.MediaId);
            if (existingLikedMedia is true)
                throw new Exception("already exist");
            var likedMedia = LikedMediaDomain.Create(
                request.MediaId,
                request.UserId

            );
            await unitOfWork.LikedMediaRepository.AddAsync(likedMedia);
            await unitOfWork.CompleteAsync();
            return LikedMediaMapper.ToResponse(likedMedia);
        }
    }
}
