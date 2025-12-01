using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.DTO.Mapper
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
            review.LastModifiedAt=DateTime.UtcNow;
        }
    }
}
