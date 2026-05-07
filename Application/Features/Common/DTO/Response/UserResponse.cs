namespace Application.Features.Common.DTO.Response
{
    public record UserDetailsResponse(Guid id, string username, string email, string name,
    string surname);
}
