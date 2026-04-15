namespace Application.Common.DTO.Response
{
    public record LikedMediaResponse(Guid id, UserDetailsResponse user, MediaResponse Media, DateTime LikedDate)
    {
    }
}
