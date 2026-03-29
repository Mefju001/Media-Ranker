using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Aggregate;
using Domain.Entity;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.LikedServices.AddLiked
{
    public class AddLikedHandler : IRequestHandler<AddLikedCommand, LikedMediaResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<AddLikedHandler> logger;
        private readonly IMediaRepository mediaRepository;
        private readonly IUserRepository userRepository;
        private readonly IGenreRepository genreRepository;
        private readonly IDirectorRepository directorRepository;
        private readonly ILikedMediaRepository likedMediaRepository;

        public AddLikedHandler(ILogger<AddLikedHandler> logger, IUnitOfWork unitOfWork, ILikedMediaRepository likedMediaRepository, IDirectorRepository directorRepository, IMediaRepository mediaRepository, IUserRepository userRepository, IGenreRepository genreRepository)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.mediaRepository = mediaRepository;
            this.userRepository = userRepository;
            this.genreRepository = genreRepository;
            this.directorRepository = directorRepository;
            this.likedMediaRepository = likedMediaRepository;
        }

        public async Task<LikedMediaResponse> Handle(AddLikedCommand request, CancellationToken cancellationToken)
        {
            var media = await mediaRepository.GetByIdAsync(request.MediaId, cancellationToken);
            var user = await userRepository.GetUserById(request.UserId, cancellationToken);
            if (media is null || user is null)
            {
                logger.LogWarning("User or Media not found. UserId: {UserId}, MediaId: {MediaId}", request.UserId, request.MediaId);
                throw new NotFoundException("User or Media not found.");
            }
            var existingLikedMedia = await likedMediaRepository.Any(request.UserId, request.MediaId, cancellationToken);
            if (existingLikedMedia is true)
            {
                logger.LogWarning("Media already in liked list. UserId: {UserId}, MediaId: {MediaId}", request.UserId, request.MediaId);
                throw new NotFoundException("Media already in liked list.");
            }
            var genre = await genreRepository.Get(media.GenreId, cancellationToken);
            if (genre is null)
            {
                logger.LogWarning("Genre not found for MediaId: {MediaId}, GenreId: {GenreId}", request.MediaId, media.GenreId);
                throw new NotFoundException("genre not found");
            }
            Director? director = null;
            if (media is Movie movie)
            {
                director = await directorRepository.Get(movie.DirectorId, cancellationToken);
            }
            var likedMedia = LikedMedia.Create(
                request.UserId,
                request.MediaId
            );
            await likedMediaRepository.AddAsync(likedMedia, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            logger.LogInformation("Media added to liked list. UserId: {UserId}, MediaId: {MediaId}", request.UserId, request.MediaId);
            return media switch
            {
                Movie m => LikedMediaMapper.ToResponse(likedMedia, user, m, genre, director!),
                Game g => LikedMediaMapper.ToResponse(likedMedia, user, g, genre),
                TvSeries tv => LikedMediaMapper.ToResponse(likedMedia, user, tv, genre),
                _ => throw new Exception("Unknown media type")
            };
        }
    }
}
