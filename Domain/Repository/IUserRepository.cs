using Domain.Entity;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<string> GetUsernameById(Guid id);
        Task<User> GetUserByUsername(string username);
        Task<Dictionary<Guid, User>>GetByIds(List<Guid> userIds);
        Task<User?> AuthenticateAsync(string username, string password);
        Task<IList<string>> getUserRoles(string username);
        Task<User> GetUserById(Guid userId);
        Task<bool> IsAnyUserWhoHaveEmailAndId(string email, Guid id);
        Task DeleteUser(Guid userId);
        Task<bool> IsAnyUserWithUsernameAndEmailLikeThat(string username, string email);
        Task CreateUserWithDefaultRole(User user);
    }
}
