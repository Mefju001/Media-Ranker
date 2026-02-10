using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using MediatR;

namespace Application.Features.LikedServices.GetAllLikedByUser
{
    public class GetAllLikedByUserHandler : IRequestHandler<GetAllLikedByUserQuery, List<LikedMediaResponse>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllLikedByUserHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<LikedMediaResponse>> Handle(GetAllLikedByUserQuery request, CancellationToken cancellationToken)
        {
            var likedItems = await unitOfWork.LikedMediaRepository.GetLikedForUser(request.userId);
            var user = await unitOfWork.UserRepository.GetUserById(request.userId);
            var mediaIds = likedItems.Select(r => r.mediaId).Distinct().ToList();
            var medias = await unitOfWork.MediaRepository.GetByIds(mediaIds);
            var genres = await unitOfWork.GenreRepository.GetGenresDictionary();
            var directors = await unitOfWork.DirectorRepository.GetDirectorsDictionary();
            var results = new List<LikedMediaResponse>();
            foreach (var item in likedItems)
            {
                var media = medias[item.mediaId];
                var genre = genres[media.GenreId];
                results.Add(media switch
                {
                    MovieDomain m => LikedMediaMapper.ToResponse(item, user, m, genre, directors[m.DirectorId]),
                    GameDomain g => LikedMediaMapper.ToResponse(item, user, g, genre),
                    TvSeriesDomain t => LikedMediaMapper.ToResponse(item, user, t, genre),
                    _ => throw new Exception("Unknown media type")
                });
            }
            return results;
        }
    }
}
