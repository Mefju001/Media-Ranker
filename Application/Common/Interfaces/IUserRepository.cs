using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserIdByUsername(string username);
    }
}
