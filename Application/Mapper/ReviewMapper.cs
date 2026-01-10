using WebApplication1.Application.Common.DTO.Request;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Application.Mapper
{
    public static class ReviewMapper
    {
        public static ReviewResponse ToResponse(Review review)
        {
            return new ReviewResponse(
                review.Id,
                review.MediaId,
                review.User.username ?? "Nieznany użytkownik",
                review.Rating,
                review.Comment,
                review.CreatedAt,
                review.LastModifiedAt);

        }
        public static void updateReview(Review review, ReviewRequest reviewRequest)
        {
            review.Rating = reviewRequest.Rating;
            review.Comment = reviewRequest.Comment;
            review.LastModifiedAt = DateTime.UtcNow;
        }
    }
}
