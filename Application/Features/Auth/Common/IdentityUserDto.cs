namespace Application.Features.Auth.Common
{
    public record IdentityUserDto(Guid Id, string Username, string Email, List<string> Roles);
}
