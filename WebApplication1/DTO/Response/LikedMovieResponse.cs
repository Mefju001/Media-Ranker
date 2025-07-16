namespace WebApplication1.DTO.Response
{
    public record LikedMovieResponse(UserResponse user, MovieResponse movie, DateTime LikedDate)
    {
    }
}
