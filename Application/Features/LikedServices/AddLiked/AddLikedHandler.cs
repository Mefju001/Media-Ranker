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
            var media = await unitOfWork.MediaRepository.GetMediaById(request.MediaId);
            var user = await  unitOfWork.UserRepository.GetUserById(request.UserId);
            var genre = await unitOfWork.GenreRepository.Get(media.GenreId);
            if (media is null || user is null||genre is null)
                throw new Exception("is empty");
            var existingLikedMedia = await unitOfWork.LikedMediaRepository.Any(request.UserId, request.MediaId);
            if (existingLikedMedia is true)
                throw new Exception("already exists");
            var likedMedia = LikedMediaDomain.Create(
                request.MediaId,
                request.UserId
            );
            await unitOfWork.LikedMediaRepository.AddAsync(likedMedia);
            await unitOfWork.CompleteAsync();
            return media switch
            {
                MovieDomain m => LikedMediaMapper.ToResponse(likedMedia,user,m,genre,await unitOfWork.DirectorRepository.Get(m.DirectorId)),
                GameDomain g=> LikedMediaMapper.ToResponse(likedMedia,user,g,genre),
                TvSeriesDomain tv=> LikedMediaMapper.ToResponse(likedMedia,user,tv,genre),
                _ => throw new Exception("invalid media type")
            };
        }
    }
}
