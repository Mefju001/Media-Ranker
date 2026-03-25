using Domain.Entity;
using Domain.Value_Object;

namespace Domain.DomainServices
{
    public interface IUserPasswordService
    {
        Password GenerateNewPassword(User user, string oldPassword, string newPassword);
    }
}
