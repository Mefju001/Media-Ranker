using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.DTO
{
    public record UserDTO(Guid Id, string Username, string Email, List<string> Roles);
}
