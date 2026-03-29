namespace Application.Common.DTO.Response
{
    public record UserResponse(Guid id, string username, string name,
    string surname, string email,
    List<RoleResponse> roles);
}
