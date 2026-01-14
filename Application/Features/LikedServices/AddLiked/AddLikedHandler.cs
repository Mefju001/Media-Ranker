using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Application.Mapper;
using WebApplication1.Domain.Entities;

namespace Application.Features.LikedServices.AddLiked
{
    public class AddLikedHandler : IRequestHandler<AddLikedCommand,LikedMediaResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public AddLikedHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<LikedMediaResponse> Handle(AddLikedCommand request, CancellationToken cancellationToken)
        {
            var media = unitOfWork.MediaRepository.GetMediaById(request.MediaId);
            var user =  unitOfWork.UserRepository.GetUserById(request.UserId);
            if (media is null || user is null)
                throw new Exception("is empty");
            var existingLikedMedia = await unitOfWork.LikedMediaRepository.Any(request.UserId, request.MediaId);
            if (existingLikedMedia is true)
                throw new Exception("already exist");
            var likedMedia = new LikedMedia
            {
                MediaId = request.MediaId,
                UserId = request.UserId,
                LikedDate = DateTime.Now,
            };
            await unitOfWork.LikedMediaRepository.AddAsync(likedMedia);
            await unitOfWork.CompleteAsync();
            return LikedMediaMapper.ToResponse(likedMedia);
        }
    }
}
