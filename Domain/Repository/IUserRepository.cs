using Domain.Entity;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> ChangePassword(Guid userId, string currentPassword, string newPassword);
        Task<string> GetUsernameById(Guid id);
        Task<User> GetUserByUsername(string username);
        Task<Dictionary<Guid, User>> GetByIds(List<Guid> userIds, CancellationToken cancellationToken);
        Task<User?> AuthenticateAsync(string username, string password);
        Task<IList<string>> getUserRoles(string username);
        Task<User> GetUserById(Guid userId, CancellationToken cancellationToken);
        Task<bool> IsAnyUserWhoHaveEmailAndId(string email, Guid id);
        Task<IdentityResult?> DeleteUser(User user);
        Task<bool> IsAnyUserWithUsernameAndEmailLikeThat(string username, string email);
        Task<User> CreateUserWithDefaultRole(User user, CancellationToken cancellationToken);
    }
}
