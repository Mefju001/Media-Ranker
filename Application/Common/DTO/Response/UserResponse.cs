namespace Application.Common.DTO.Response
{
    public record UserResponse(Guid id, string username, string password, string name,
    string surname, string email,
    List<RoleResponse> roles);
}
