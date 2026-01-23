using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDomain> GetUserIdByUsername(string username);
        Task<UserDomain> GetUserById(int userId);
        Task<bool> IsAnyUserWhoHaveEmailAndId(string email, int id);
        Task DeleteUser(int userId);
        Task<bool> IsAnyUserWithUsernameAndEmailLikeThat(string username, string email);
        Task<UserDomain>AddUser(UserDomain user);
    }
}
