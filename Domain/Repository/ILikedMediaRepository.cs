using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface ILikedMediaRepository
    {
        Task<bool> Any(Guid userId, int mediaId);
        Task AddAsync(LikedMedia likedMedia);
        Task<LikedMedia?> GetByUserAndMediaId(Guid userId, int mediaId);
        Task<bool> DeleteByLikedMedia(Guid userId, int mediaId);
        Task<LikedMedia?> GetById(int likedMediaId, CancellationToken cancellationToken);
        Task<List<LikedMedia>> GetLikedForUser(Guid userId);
        Task<List<LikedMedia>> GetAll();
    }
}
