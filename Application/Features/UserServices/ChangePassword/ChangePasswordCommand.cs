using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.UserServices.ChangePassword
{
    public record ChangePasswordCommand(string newPassword, string oldPassword, string confirmPassword, int userId) : IRequest<Unit>;
}
