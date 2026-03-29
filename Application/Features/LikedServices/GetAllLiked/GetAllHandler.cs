using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Aggregate;
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
            var likedItems = await likedMediaRepository.GetAll(cancellationToken);
            if (!likedItems.Any()) return new List<LikedMediaResponse>();

            var userIds = likedItems.Select(x => x.UserId).Distinct().ToList();
            var mediaIds = likedItems.Select(x => x.MediaId).Distinct().ToList();

            var users = await userRepository.GetByIds(userIds, cancellationToken);
            var mediaList = await mediaRepository.GetByIds(mediaIds, cancellationToken);
            //zwracac genre i directors poprzez id wcześniej wyszukane
            var genres = await genreRepository.GetGenresDictionary(cancellationToken);
            var directors = await directorRepository.GetDirectorsDictionary(cancellationToken);

            var result = new List<LikedMediaResponse>();
            foreach (var lm in likedItems)
            {
                if (!users.TryGetValue(lm.UserId, out var user) ||
                   !mediaList.TryGetValue(lm.MediaId, out var media))
                    continue;
                genres.TryGetValue(media.GenreId, out var genre);
                result.Add(media switch
                {
                    Movie m => LikedMediaMapper.ToResponse(lm, user, m, genre, directors.TryGetValue(m.DirectorId, out var director) ? director : null),
                    Game g => LikedMediaMapper.ToResponse(lm, user, g, genre),
                    TvSeries t => LikedMediaMapper.ToResponse(lm, user, t, genre),
                    _ => throw new Exception("Unknown media type")
                });
            }
            return result;
        }
    }
}
