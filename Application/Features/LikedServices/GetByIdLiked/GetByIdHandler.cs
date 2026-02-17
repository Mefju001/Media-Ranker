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
        public GetByIdHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<LikedMediaResponse?> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var liked = await unitOfWork.LikedMediaRepository.GetById(request.id);
            if (liked == null) throw new NotFoundException("Liked media not found");

            var mediaTask = unitOfWork.MediaRepository.GetMediaById(liked.mediaId);
            var userTask = unitOfWork.UserRepository.GetUserById(liked.userId);

            await Task.WhenAll(mediaTask, userTask);

            var media = await mediaTask;
            var user = await userTask;

            if (media == null) throw new NotFoundException("Associated media not found");

            var genre = await unitOfWork.GenreRepository.Get(media.GenreId);
            if(genre is null) throw new NotFoundException("Genre not found");

            return media switch
            {
                Movie m => LikedMediaMapper.ToResponse(liked, user, m, genre, await unitOfWork.DirectorRepository.Get(m.DirectorId)),
                Game g => LikedMediaMapper.ToResponse(liked, user, g, genre),
                TvSeries t => LikedMediaMapper.ToResponse(liked, user, t, genre),
                _ => throw new InvalidOperationException("Unknown media type")
            };
        }
    }
}
