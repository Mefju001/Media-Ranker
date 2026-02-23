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
        private readonly IMediaRepository mediaRepository;
        private readonly IUserRepository userRepository;
        private readonly IGenreRepository genreRepository;
        private readonly IDirectorRepository directorRepository;
        private readonly ILikedMediaRepository likedMediaRepository;

        public GetAllLikedByUserHandler(IUnitOfWork unitOfWork, IMediaRepository mediaRepository, IUserRepository userRepository, IGenreRepository genreRepository, IDirectorRepository directorRepository, ILikedMediaRepository likedMediaRepository)
        {
            this.unitOfWork = unitOfWork;
            this.likedMediaRepository = likedMediaRepository;
            this.userRepository = userRepository;
            this.mediaRepository = mediaRepository;
            this.genreRepository = genreRepository;
            this.directorRepository = directorRepository;
        }

        public async Task<List<LikedMediaResponse>> Handle(GetAllLikedByUserQuery request, CancellationToken cancellationToken)
        {
            var likedItems = await likedMediaRepository.GetLikedForUser(request.userId);
            var user = await userRepository.GetUserById(request.userId);
            var mediaIds = likedItems.Select(r => r.mediaId).Distinct().ToList();
            var medias = await mediaRepository.GetByIds(mediaIds);
            var genres = await genreRepository.GetGenresDictionary();
            var directors = await directorRepository.GetDirectorsDictionary();
            var results = new List<LikedMediaResponse>();
            foreach (var item in likedItems)
            {
                var media = medias[item.mediaId];
                var genre = genres[media.GenreId];
                results.Add(media switch
                {
                    Movie m => LikedMediaMapper.ToResponse(item, user, m, genre, directors[m.DirectorId]),
                    Game g => LikedMediaMapper.ToResponse(item, user, g, genre),
                    TvSeries t => LikedMediaMapper.ToResponse(item, user, t, genre),
                    _ => throw new Exception("Unknown media type")
                });
            }
            return results;
        }
    }
}
