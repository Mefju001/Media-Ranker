
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using WebApplication1.Data;
using WebApplication1.DTO.Mapper;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Exceptions;
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
        public async Task<ReviewResponse> Upsert(int? reviewId, int userId, int movieId, ReviewRequest reviewRequest)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                Review? review= null;
                if (reviewId.HasValue)
                {
                    review = await unitOfWork.Reviews.AsQueryable()
                       .FirstOrDefaultAsync(r => r.Id == reviewId&&r.UserId == userId);
                    if (review is null)
                    {
                        throw new NotFoundException($"Review with this id {reviewId} was not found or does not belong to the user. ");
                    }
                    ReviewMapper.updateReview(review, reviewRequest);
                }
                else
                {
                    review = new Review
                    {
                        Comment = reviewRequest.Comment,
                        Rating = reviewRequest.Rating,
                        MediaId = movieId,
                        UserId = userId
                    };
                    await unitOfWork.Reviews.AddAsync(review);
                }
                await unitOfWork.CompleteAsync();
                var response = ReviewMapper.ToResponse(review)?? throw new InvalidOperationException("The saved review could not be found.");
                await transaction.CommitAsync();
                return response;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> Delete(int id,int userId)
        {
            if (id < 0) throw new ArgumentOutOfRangeException("id");
            var review = await unitOfWork.Reviews.FirstOrDefaultAsync(r => r.Id == id&&r.UserId == userId);
            if (review is null)
            {
                return false;
            }
            unitOfWork.Reviews.Delete(review);
            await unitOfWork.CompleteAsync();
            return true;
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
                .OrderByDescending(r=>r.CreatedAt)
                .Take(10)
                .Select(r=>r.Media.title)
                .Distinct()
                .ToListAsync();
            return title;
        }
        public async Task<ReviewResponse?> GetById(int id)
        {
            var review = await unitOfWork.Reviews.AsQueryable()
                .Include(r => r.User)
                .Include(r => r.Media)
                .FirstOrDefaultAsync(r => r.Id == id);
            return review is null? null: ReviewMapper.ToResponse(review);
        }

    }
}
