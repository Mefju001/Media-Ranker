namespace Application.Common.DTO.Response
{
    public record LikedMediaResponse(int id, UserDetailsResponse user, MediaResponse Media, DateTime LikedDate)
    {
    }
}
