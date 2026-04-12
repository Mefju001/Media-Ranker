using Application.Common.DTO;
using Domain.Aggregate;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> ChangePassword(Guid userId, string currentPassword, string newPassword);
        Task<string> GetUsernameById(Guid id, CancellationToken cancellationToken);
        Task<UserDTO?> AuthenticateAsync(string username, string password);
        Task<UserDTO> GetUserById(Guid userId, CancellationToken cancellationToken);
        Task<bool> IsAnyUserWhoHaveEmailAndId(string email, Guid id);
        Task<IdentityResult?> DeleteUser(Guid id);
        Task<bool> IsAnyUserWithUsernameAndEmailLikeThat(string username, string email);
        Task<UserDTO> CreateUserWithDefaultRole(string username, string password, string email, CancellationToken cancellationToken);
    }
}
