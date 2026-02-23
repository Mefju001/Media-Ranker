using Domain.Entity;
using Domain.Value_Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainServices
{
    public interface IUserPasswordService
    {
        Password GenerateNewPassword(User user, string oldPassword, string newPassword);
    }
}
