using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IReviewRepository
    {
        Task<ReviewDomain?> GetReviewByIdAsync(int reviewId);
        Task<ReviewDomain> AddAsync(ReviewDomain review);
        Task<List<string>> GetTheLastestReviewAsync(CancellationToken cancellationToken);
        Task<List<ReviewDomain>> GetAllReviewsAsync(CancellationToken cancellation);
        Task DeleteAsync(ReviewDomain review);
    }
}
