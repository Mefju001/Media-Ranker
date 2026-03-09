using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface ILikedMediaRepository
    {
        Task<bool> Any(Guid userId, int mediaId, CancellationToken cancellationToken);
        Task AddAsync(LikedMedia likedMedia, CancellationToken cancellationToken);
        Task<LikedMedia?> GetByUserAndMediaId(Guid userId, int mediaId, CancellationToken cancellationToken);
        Task<bool> DeleteByLikedMedia(Guid userId, int mediaId, CancellationToken cancellationToken);
        Task<LikedMedia?> GetById(int likedMediaId, CancellationToken cancellationToken);
        Task<List<LikedMedia>> GetLikedForUser(Guid userId, CancellationToken cancellationToken);
        Task<List<LikedMedia>> GetAll(CancellationToken cancellationToken);
    }
}
