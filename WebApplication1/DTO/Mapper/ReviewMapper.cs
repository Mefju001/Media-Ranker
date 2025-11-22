using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.DTO.Mapping
{
    public static class ReviewMapper
    {
        public static ReviewResponse ToResponse(Review review)
        {
            return new ReviewResponse(
                review.User.username ?? "Nieznany użytkownik",
                review.Rating,
                review.Comment);

        }
    }
}
