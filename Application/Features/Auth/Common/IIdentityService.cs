
namespace Application.Features.Auth.Common
{
    public interface IIdentityService
    {
        Task<List<Guid>> GetAdminsIds();
        Task<IdentityUserDto> CreateUserWithDefaultRole(string username, string password, string email);
        Task<bool> IsAnyUserWhoHaveEmailAndId(string email, string username, CancellationToken cancellationToken);
        Task<IdentityUserDto?> AuthenticateAsync(string username, string password);
        Task<IdentityUserDto> GetUserById(Guid userId, CancellationToken cancellationToken);
        Task ChangePassword(Guid userId, string currentPassword, string newPassword);
        Task DeleteUser(Guid id);
    }
}
