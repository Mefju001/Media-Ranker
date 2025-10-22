using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO.Mapping;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class LikedMediaServices(AppDbContext context) : ILikedMediaServices
    {
        private readonly AppDbContext AppDbContext = context;

        public async Task<LikedMediaResponse> Add(LikedMediaRequest request)
        {
            var media = await AppDbContext.Medias.FirstOrDefaultAsync(m => m.Id == request.MediaId);
            var user = await AppDbContext.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (media is null || user is null)//wyjatek
                throw new Exception("Jest pusty");
            var existingLikedMedia = await AppDbContext.LikedMedias.FirstOrDefaultAsync(lm => lm.UserId == request.UserId && lm.MediaId == request.MediaId);
            if (existingLikedMedia is not null)//wyjatek 
                throw new Exception("istnieje takie polubienie");
            var likedMedia = new LikedMedia
            {
                MediaId = request.MediaId,
                UserId = request.UserId,
                LikedDate = DateTime.Now,
            };
            AppDbContext.LikedMedias.Add(likedMedia);
            await AppDbContext.SaveChangesAsync();
            return LikedMediaMapping.ToResponse(likedMedia);
        }

        public async Task<bool> Delete(int userId, int mediaId)
        {
            var itemToDelete = await AppDbContext.LikedMedias.FirstOrDefaultAsync(lm => lm.UserId == userId && lm.MediaId == mediaId);
            if (itemToDelete is null)
            {
                //Dodać nowy wyjatek dla pustych polubionych.
                throw new Exception("Jest pusty");

            }
            AppDbContext.LikedMedias.Remove(itemToDelete);
            await AppDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<LikedMediaResponse>> GetAllAsync()
        {
            var likedItems = await AppDbContext.LikedMedias
                .ToListAsync();
            return likedItems.Select(LikedMediaMapping.ToResponse).ToList();
        }
        public async Task<List<LikedMediaResponse>> GetUserLikedMedia(int userId)
        {
            var likedItems = await AppDbContext.LikedMedias
                .Where(lm => lm.UserId == userId)
                .ToListAsync();
            return likedItems.Select(LikedMediaMapping.ToResponse).ToList();
        }

    }
}
