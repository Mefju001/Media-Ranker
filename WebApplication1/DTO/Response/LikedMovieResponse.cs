namespace WebApplication1.DTO.Response
{
    public record LikedMediaResponse(UserResponse user, MovieResponse movie, DateTime LikedDate)
    {
    }
}
