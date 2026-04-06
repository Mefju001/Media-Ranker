using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainService
{
    public interface IPasswordHasher
    {
        string CreatePasswordHash(string password);
    }
}
