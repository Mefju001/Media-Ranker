namespace Application.Features.Common.DTO
{
    public record UserDTO(Guid Id, string Username, string Email, List<string> Roles);
}
