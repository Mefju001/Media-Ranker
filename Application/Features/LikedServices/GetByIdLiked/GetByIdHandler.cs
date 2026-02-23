using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.LikedServices.GetByIdLiked
{
    public class GetByIdHandler : IRequestHandler<GetByIdQuery, LikedMediaResponse?>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILikedMediaRepository likedMediaRepository;
        private readonly IMediaRepository mediaRepository;
        private readonly IUserRepository userRepository;
        private readonly IGenreRepository genreRepository;
        private readonly IDirectorRepository directorRepository;
        public GetByIdHandler(IUnitOfWork unitOfWork, IDirectorRepository directorRepository, IGenreRepository genreRepository, ILikedMediaRepository likedMediaRepository, IMediaRepository mediaRepository, IUserRepository userRepository)
        {
            this.unitOfWork = unitOfWork;
            this.likedMediaRepository = likedMediaRepository;
            this.mediaRepository = mediaRepository;
            this.userRepository = userRepository;
            this.genreRepository = genreRepository;
            this.directorRepository = directorRepository;
        }

        public async Task<LikedMediaResponse?> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var liked = await likedMediaRepository.GetById(request.id);
            if (liked == null) throw new NotFoundException("Liked media not found");

            var mediaTask = mediaRepository.GetMediaById(liked.mediaId);
            var userTask = userRepository.GetUserById(liked.userId);

            await Task.WhenAll(mediaTask, userTask);

            var media = await mediaTask;
            var user = await userTask;

            if (media == null) throw new NotFoundException("Associated media not found");

            var genre = await genreRepository.Get(media.GenreId);
            if(genre is null) throw new NotFoundException("Genre not found");

            return media switch
            {
                Movie m => LikedMediaMapper.ToResponse(liked, user, m, genre, await directorRepository.Get(m.DirectorId)),
                Game g => LikedMediaMapper.ToResponse(liked, user, g, genre),
                TvSeries t => LikedMediaMapper.ToResponse(liked, user, t, genre),
                _ => throw new InvalidOperationException("Unknown media type")
            };
        }
    }
}
