using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IMediaRepository
    {
        Task<Media> GetMediaById(int mediaId);
        Task<Dictionary<int, Media>> GetByIds(List<int> mediaIds);
    }
}
