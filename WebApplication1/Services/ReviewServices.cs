
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO.Mapper;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class ReviewServices : IReviewServices
    {
        private readonly IUnitOfWork unitOfWork;
        public ReviewServices(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<(int reviewId, ReviewResponse response)> Upsert(int? reviewId, int userId, int movieId, ReviewRequest reviewRequest)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                Review? review;
                if (reviewId is not null)
                {
                    review = await unitOfWork.Reviews.AsQueryable()
                       .Include(r => r.Media)
                       .Include(r => r.User)
                       .FirstOrDefaultAsync(r => r.Id == reviewId);
                    if (review is not null)
                    {
                        review.Rating = reviewRequest.Rating;
                        review.Comment = reviewRequest.Comment;
                        await unitOfWork.CompleteAsync();
                        await transaction.CommitAsync();
                        return (review.Id, ReviewMapper.ToResponse(review));
                    }
                }
                review = new Review
                {
                    Comment = reviewRequest.Comment,
                    Rating = reviewRequest.Rating,
                    MediaId = movieId,
                    UserId = userId
                };
                await unitOfWork.Reviews.AddAsync(review);
                await unitOfWork.CompleteAsync();
                var response = await unitOfWork.Reviews.AsQueryable()
                    .Include(r => r.User)
                    .Include(r => r.Media)
                    .FirstOrDefaultAsync(r => r.Id == review.Id);
                await transaction.CommitAsync();
                return (review.Id, ReviewMapper.ToResponse(response));
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var review = await unitOfWork.Reviews.FirstOrDefaultAsync(r => r.Id == id);
            if (review != null)
            {
                unitOfWork.Reviews.Delete(review);
                await unitOfWork.CompleteAsync();
                return true;
            }
            return false;
        }

        public async Task<List<ReviewResponse>> GetAllAsync()
        {
            var reviews = await unitOfWork.Reviews.AsQueryable()
                .Include(r => r.User)
                .Include(r => r.Media)
                .ToListAsync();

            return reviews.Select(ReviewMapper.ToResponse).ToList();
        }
        public async Task<List<string>>GetTheLatestReviews()
        {
            var title = await unitOfWork.Reviews.AsQueryable()
                .Include(r => r.User)
                .Include(r => r.Media)
                .Take(10)
                .OrderByDescending(r=>r.CreatedAt)
                .Select(r=>r.Media.title)
                .Distinct()
                .ToListAsync();
            return title;
        }
        public async Task<ReviewResponse> GetById(int id)
        {
            var review = await unitOfWork.Reviews.AsQueryable()
                .Include(r => r.User)
                .Include(r => r.Media)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (review != null) return ReviewMapper.ToResponse(review);
            return null;
        }

    }
}
