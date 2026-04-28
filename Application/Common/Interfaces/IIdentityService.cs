using Application.Common.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<UserDTO> CreateUserWithDefaultRole(string username, string password, string email);
        Task<bool> IsAnyUserWhoHaveEmailAndId(string email, string username, CancellationToken cancellationToken);
        Task<UserDTO?> AuthenticateAsync(string username, string password);
        Task<UserDTO> GetUserById(Guid userId, CancellationToken cancellationToken);
        Task ChangePassword(Guid userId, string currentPassword, string newPassword);
        Task DeleteUser(Guid id);
    }
}
