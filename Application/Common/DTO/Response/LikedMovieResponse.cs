namespace Application.Common.DTO.Response
{
    public record LikedMediaResponse(int id, UserResponse user, MediaResponse Media, DateTime LikedDate)
    {
    }
}
