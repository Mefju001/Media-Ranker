using WebApplication1.Application.Common.DTO.Request;
using WebApplication1.Application.Common.DTO.Response;

namespace WebApplication1.Services.Interfaces
{
    public interface IUserServices
    {
        Task<List<UserResponse>> GetAllAsync();
        Task<UserResponse?> GetById(int id);
        Task Delete(int id);
        Task changePassword(string newPassword, string confirmPassword, string oldPassword, int userId);
        Task changedetails(int userId, UserDetailsRequest userDetailsRequest);
        Task<bool> Register(UserRequest userRequest);
        Task<List<UserResponse>> GetBy(string name);
    }
}
