using Microsoft.EntityFrameworkCore;
using WebApplication1.Application.Common.DTO.Request;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Application.Mapper;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class LikedMediaServices : ILikedMediaServices
    {
        private readonly IUnitOfWork unitOfWork;
        public LikedMediaServices(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<LikedMediaResponse> Add(LikedMediaRequest request)
        {
            var media = await unitOfWork.Medias.FirstOrDefaultAsync(m => m.Id == request.MediaId);
            var user = await unitOfWork.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (media is null || user is null)
                throw new Exception("is empty");
            var existingLikedMedia = await unitOfWork.LikedMedias.FirstOrDefaultAsync(lm => lm.UserId == request.UserId && lm.MediaId == request.MediaId);
            if (existingLikedMedia is not null)
                throw new Exception("already exist");
            var likedMedia = new LikedMedia
            {
                MediaId = request.MediaId,
                UserId = request.UserId,
                LikedDate = DateTime.Now,
            };
            await unitOfWork.LikedMedias.AddAsync(likedMedia);
            await unitOfWork.CompleteAsync();
            return LikedMediaMapper.ToResponse(likedMedia);
        }

        public async Task<bool> Delete(int userId, int mediaId)
        {
            var itemToDelete = await unitOfWork.LikedMedias.FirstOrDefaultAsync(lm => lm.UserId == userId && lm.MediaId == mediaId);
            if (itemToDelete is null)
            {
                throw new Exception("is empty");

            }
            unitOfWork.LikedMedias.Delete(itemToDelete);
            await unitOfWork.CompleteAsync();
            return true;
        }
        public async Task<LikedMediaResponse?> GetBy(int likedId)
        {
            var liked = await unitOfWork.LikedMedias.FirstOrDefaultAsync(l => l.Id == likedId);
            return liked == null ? null : LikedMediaMapper.ToResponse(liked);
        }
        public async Task<List<LikedMediaResponse>> GetAllAsync()
        {
            var likedItems = await unitOfWork.LikedMedias.AsQueryable().ToListAsync();
            return likedItems.Select(LikedMediaMapper.ToResponse).ToList();
        }
        public async Task<List<LikedMediaResponse>> GetUserLikedMedia(int userId)
        {
            var likedItems = await unitOfWork.LikedMedias.AsQueryable()
                .Where(lm => lm.UserId == userId)
                .ToListAsync();
            return likedItems.Select(LikedMediaMapper.ToResponse).ToList();
        }

    }
}
