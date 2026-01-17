using Application.Common.DTO.Request;
using Application.Common.DTO.Response;
using Domain.Entity;

namespace Application.Mapper
{
    public static class ReviewMapper
    {
        public static ReviewResponse ToResponse(ReviewDomain review)
        {
            return new ReviewResponse(
                review.Id,
                review.MediaId,
                "",//review.User.username ?? "Nieznany użytkownik",
                review.Rating,
                review.Comment,
                review.CreatedAt,
                review.LastModifiedAt);

        }

    }
}
