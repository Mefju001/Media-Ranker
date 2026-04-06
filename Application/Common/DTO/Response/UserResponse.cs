namespace Application.Common.DTO.Response
{
    public record UserDetailsResponse(Guid id, string name,
    string surname, string email);
}
