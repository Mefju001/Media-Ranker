using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IMediaRepository
    {
        Task<MediaDomain> GetMediaById(int mediaId);
    }
}
