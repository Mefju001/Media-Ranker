namespace WebApplication1.DTO.Response
{
    public record UserResponse(string username, string password, string name,
    string surname, string email,
    List<RoleResponse> role, List<ReviewResponse> Reviews);
}
