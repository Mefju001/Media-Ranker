using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.DTO.Mapping
{
    public static class ReviewMapping
    {
        public static ReviewResponse ToResponse(Review review)
        {
            return new ReviewResponse(
                review.Media?.title?? "Brak tytułu", 
                review.User?.username?? "Nieznany użytkownik", 
                review.Rating, 
                review.Comment);

    }
    }
}
