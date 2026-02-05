using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using MediatR;

namespace Application.Features.LikedServices.GetAllLiked
{
    public class GetAllHandler : IRequestHandler<GetAllQuery, List<LikedMediaResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAllHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<List<LikedMediaResponse>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var likedItems = await unitOfWork.LikedMediaRepository.GetAll();
            var userIds = likedItems.Select(x => x.userId).Distinct().ToList();
            var mediaIds = likedItems.Select(x => x.mediaId).Distinct().ToList();
            var users = await unitOfWork.UserRepository.GetByIds(userIds);
            var mediaList = await unitOfWork.MediaRepository.GetByIds(mediaIds);
            var genres = await unitOfWork.GenreRepository.GetGenresDictionary();
            var directors = await unitOfWork.DirectorRepository.GetDirectorsDictionary();
            var result = new List<LikedMediaResponse>();
            foreach (var lm in likedItems) {
                var user = users[lm.userId];
                var media = mediaList[lm.mediaId];
                var genre = genres[media.GenreId];
                result.Add(media switch
                {
                    MovieDomain m=> LikedMediaMapper.ToResponse(lm, user, m, genre, directors[m.DirectorId]),
                    GameDomain g => LikedMediaMapper.ToResponse(lm, user, g, genre),
                    TvSeriesDomain t => LikedMediaMapper.ToResponse(lm, user, t, genre),
                    _ => throw new Exception("Unknown media type")
                });
            }
            return result;
        }
    }
}
