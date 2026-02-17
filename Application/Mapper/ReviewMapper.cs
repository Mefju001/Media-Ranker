using Application.Common.DTO.Response;
using Domain.Entity;

namespace Application.Mapper
{
    public static class ReviewMapper
    {
        public static ReviewResponse ToResponse(Review review)
        {
            return new ReviewResponse(
                review.Id,
                review.MediaId,
                review.UserId.ToString(),
                review.Rating.value,
                review.Comment,
                review.CreatedAt,
                review.LastModifiedAt);

        }

    }
}
