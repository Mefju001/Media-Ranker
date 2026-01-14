using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.UserServices.DeleteUser
{
    public record DeleteUserCommand(int id) : IRequest<Unit>;
}
