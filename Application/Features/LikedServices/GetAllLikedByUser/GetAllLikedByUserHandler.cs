using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using MediatR;
using Domain.Exceptions;

namespace Application.Features.LikedServices.GetAllLikedByUser
{
    public class GetAllLikedByUserHandler : IRequestHandler<GetAllLikedByUserQuery, List<LikedMediaResponse>>
    {
        private readonly IMediaRepository mediaRepository;
        private readonly IUserRepository userRepository;
        private readonly IGenreRepository genreRepository;
        private readonly IDirectorRepository directorRepository;
        private readonly ILikedMediaRepository likedMediaRepository;

        public GetAllLikedByUserHandler(IMediaRepository mediaRepository, IUserRepository userRepository, IGenreRepository genreRepository, IDirectorRepository directorRepository, ILikedMediaRepository likedMediaRepository)
        {
            this.likedMediaRepository = likedMediaRepository;
            this.userRepository = userRepository;
            this.mediaRepository = mediaRepository;
            this.genreRepository = genreRepository;
            this.directorRepository = directorRepository;
        }

        public async Task<List<LikedMediaResponse>> Handle(GetAllLikedByUserQuery request, CancellationToken cancellationToken)
        {
            var likedItems = await likedMediaRepository.GetLikedForUser(request.userId, cancellationToken);
            if(!likedItems.Any())
                return new List<LikedMediaResponse>();
            var user = await userRepository.GetUserById(request.userId,cancellationToken);
            if (user == null) throw new NotFoundException("User not found");
            var mediaIds = likedItems.Select(r => r.mediaId).Distinct().ToList();
            var medias = await mediaRepository.GetByIds(mediaIds, cancellationToken);
            var genresIds = medias.Values.Select(m=>m.GenreId).Distinct().ToList();
            var genres = await genreRepository.GetByIdsAsync(genresIds,cancellationToken);
            var directorIds = medias.Values.OfType<Movie>().Select(m => m.DirectorId).Distinct().ToList();
            var directors = await directorRepository.GetByIds(directorIds,cancellationToken);
            var results = new List<LikedMediaResponse>();
            foreach (var item in likedItems)
            {
                if (!medias.TryGetValue(item.mediaId, out var media)) continue;

                genres.TryGetValue(media.GenreId, out var genre);

                results.Add(media switch
                {
                    Movie m => LikedMediaMapper.ToResponse(item, user, m, genre!,
                        directors.TryGetValue(m.DirectorId, out var d) ? d : null!),
                    Game g => LikedMediaMapper.ToResponse(item, user, g, genre!),
                    TvSeries t => LikedMediaMapper.ToResponse(item, user, t, genre!),
                    _ => throw new InvalidOperationException($"Unknown media type for ID: {media.Id}")
                });
            }
            return results;
        }
    }
}
