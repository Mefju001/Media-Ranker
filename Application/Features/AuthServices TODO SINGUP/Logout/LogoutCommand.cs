using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AuthServices.Logout
{
    public record LogoutCommand(string stringUserId) : IRequest;
}
