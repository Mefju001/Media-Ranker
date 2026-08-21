using Domain.Entity;

namespace Application.Features.UserInteractions.Reviews.Common
{
    public static class ReviewMapper
    {
        public static ReviewResponse ToResponse(Review review)
        {
            return new ReviewResponse(
                review.Id,
                review.MediaId,
                review.Username,
                review.Rating,
                review.Comment,
                review.AuditInfo.CreatedAt,
                review.AuditInfo.UpdatedAt);
        }

    }
}
