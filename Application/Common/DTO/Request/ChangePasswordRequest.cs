using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.DTO.Request
{
    public record ChangePasswordRequest(string newPassword, string confirmPassword, string oldPassword);
}
