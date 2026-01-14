using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface ILikedMediaRepository
    {
        Task<bool> Any(int userId, int mediaId);
        Task AddAsync(LikedMedia likedMedia);
        Task<LikedMedia?> GetByUserAndMediaId(int userId, int mediaId);
        Task<bool> DeleteByLikedMedia(int userId, int mediaId);
        Task<LikedMedia?> GetById(int likedMediaId);
        Task<List<LikedMedia>> GetLikedForUser(int userId);
        Task<List<LikedMedia>> GetAll();
    }
}
