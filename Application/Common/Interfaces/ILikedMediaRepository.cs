using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface ILikedMediaRepository
    {
        Task<bool> Any(int userId, int mediaId);
        Task AddAsync(LikedMediaDomain likedMedia);
        Task<LikedMediaDomain?> GetByUserAndMediaId(int userId, int mediaId);
        Task<bool> DeleteByLikedMedia(int userId, int mediaId);
        Task<LikedMediaDomain?> GetById(int likedMediaId);
        Task<List<LikedMediaDomain>> GetLikedForUser(int userId);
        Task<List<LikedMediaDomain>> GetAll();
    }
}
