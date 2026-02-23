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
        private readonly IMediaRepository mediaRepository;
        private readonly IUserRepository userRepository;
        private readonly IGenreRepository genreRepository;
        private readonly IDirectorRepository directorRepository;
        private readonly ILikedMediaRepository likedMediaRepository;
        public GetAllHandler(IUnitOfWork unitOfWork, ILikedMediaRepository likedMediaRepository, IMediaRepository mediaRepository, IUserRepository userRepository, IGenreRepository genreRepository, IDirectorRepository directorRepository)
        {
            this.unitOfWork = unitOfWork;
            this.mediaRepository = mediaRepository;
            this.genreRepository = genreRepository;
            this.directorRepository = directorRepository;
            this.userRepository = userRepository;
            this.likedMediaRepository = likedMediaRepository;
        }
        public async Task<List<LikedMediaResponse>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var likedItems = await likedMediaRepository.GetAll();
            var userIds = likedItems.Select(x => x.userId).Distinct().ToList();
            var mediaIds = likedItems.Select(x => x.mediaId).Distinct().ToList();
            var users = await userRepository.GetByIds(userIds);
            var mediaList = await mediaRepository.GetByIds(mediaIds);
            var genres = await genreRepository.GetGenresDictionary();
            var directors = await directorRepository.GetDirectorsDictionary();
            var result = new List<LikedMediaResponse>();
            foreach (var lm in likedItems) {
                var user = users[lm.userId];
                var media = mediaList[lm.mediaId];
                var genre = genres[media.GenreId];
                result.Add(media switch
                {
                    Movie m=> LikedMediaMapper.ToResponse(lm, user, m, genre, directors[m.DirectorId]),
                    Game g => LikedMediaMapper.ToResponse(lm, user, g, genre),
                    TvSeries t => LikedMediaMapper.ToResponse(lm, user, t, genre),
                    _ => throw new Exception("Unknown media type")
                });
            }
            return result;
        }
    }
}
