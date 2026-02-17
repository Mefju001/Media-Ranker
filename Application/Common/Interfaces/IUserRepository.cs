using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsername(string username);
        Task<Dictionary<int, User>>GetByIds(List<int>userIds);
        Task<User> GetUserIdByUsername(string username);
        Task<User> GetUserById(int userId);
        Task<bool> IsAnyUserWhoHaveEmailAndId(string email, int id);
        Task DeleteUser(int userId);
        Task<bool> IsAnyUserWithUsernameAndEmailLikeThat(string username, string email);
        Task<User> AddUser(User user);
    }
}
