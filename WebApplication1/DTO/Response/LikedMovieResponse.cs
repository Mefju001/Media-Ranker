namespace WebApplication1.DTO.Response
{
    public record LikedMediaResponse(UserResponse user, MediaResponse Media, DateTime LikedDate)
    {
    }
}
