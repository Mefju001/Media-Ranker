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
        private readonly IMediaRepository mediaRepository;
        private readonly IUserRepository userRepository;
        private readonly IGenreRepository genreRepository;
        private readonly IDirectorRepository directorRepository;
        private readonly ILikedMediaRepository likedMediaRepository;

        public AddLikedHandler(IUnitOfWork unitOfWork, ILikedMediaRepository likedMediaRepository, IDirectorRepository directorRepository, IMediaRepository mediaRepository, IUserRepository userRepository, IGenreRepository genreRepository )
        {
            this.unitOfWork = unitOfWork;
            this.mediaRepository = mediaRepository;
            this.userRepository = userRepository;
            this.genreRepository = genreRepository;
            this.directorRepository = directorRepository;
            this.likedMediaRepository = likedMediaRepository;
        }

        public async Task<LikedMediaResponse> Handle(AddLikedCommand request, CancellationToken cancellationToken)
        {
            var media = await mediaRepository.GetMediaById(request.MediaId);
            var user = await  userRepository.GetUserById(request.UserId);
            var genre = await genreRepository.Get(media.GenreId);
            if (media is null || user is null||genre is null)
                throw new Exception("is empty");
            var existingLikedMedia = await  likedMediaRepository.Any(request.UserId, request.MediaId);
            if (existingLikedMedia is true)
                throw new Exception("already exists");
            var likedMedia = LikedMedia.Create(
                request.UserId,
                request.MediaId
            );
            await likedMediaRepository.AddAsync(likedMedia);
            await unitOfWork.CompleteAsync();
            return media switch
            {
                Movie m => LikedMediaMapper.ToResponse(likedMedia,user,m,genre,await directorRepository.Get(m.DirectorId)),
                Game g=> LikedMediaMapper.ToResponse(likedMedia,user,g,genre),
                TvSeries tv=> LikedMediaMapper.ToResponse(likedMedia,user,tv,genre),
                _ => throw new Exception("invalid media type")
            };
        }
    }
}
