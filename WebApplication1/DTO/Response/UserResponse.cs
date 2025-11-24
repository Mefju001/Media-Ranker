namespace WebApplication1.DTO.Response
{
    public record UserResponse(int id, string username, string password, string name,
    string surname, string email,
    List<RoleResponse> role, List<ReviewResponse> Reviews);
}
