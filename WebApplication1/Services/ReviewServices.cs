
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO.Mapping;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class ReviewServices : IReviewServices
    {
        private readonly AppDbContext _context;
        public ReviewServices(AppDbContext context)
        {
            _context = context;
        }
        public async Task<(int reviewId, ReviewResponse response)> Upsert(int? reviewId, int userId, int movieId, ReviewRequest reviewRequest)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try 
            {
                Review? review;
                if (reviewId is not null)
                {
                    review = await _context.Reviews
                       .Include(r => r.Media)
                       .Include(r => r.User)
                       .FirstOrDefaultAsync(r => r.Id == reviewId);
                    if (review is not null)
                    {
                        review.Rating = reviewRequest.Rating;
                        review.Comment = reviewRequest.Comment;
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return (review.Id, ReviewMapping.ToResponse(review));
                    }
                }
                review = new Review
                {
                    Comment = reviewRequest.Comment,
                    Rating = reviewRequest.Rating,
                    MediaId = movieId,
                    UserId = userId
                };
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();
                var response = await _context.Reviews
                    .Include(r => r.User)
                    .Include(r => r.Media)
                    .FirstOrDefaultAsync(r => r.Id == review.Id);
                await transaction.CommitAsync();
                return (review.Id,ReviewMapping.ToResponse(response));
            }catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var review = _context.Reviews.FirstOrDefault(r => r.Id == id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<ReviewResponse>> GetAllAsync()
        {
            var reviews = await _context.Reviews
                .Include(r=>r.User)
                .Include(r=>r.Media)
                .ToListAsync();

            return reviews.Select(ReviewMapping.ToResponse).ToList();
        }

        public async Task<ReviewResponse> GetById(int id)
        {
            var review = await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Media)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (review != null)return ReviewMapping.ToResponse(review);
            return null;
        }

    }
}
