using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Application.Common.DTO.Response;

namespace Application.Features.AuthServices.Login
{
    public record LoginCommand(string username, string password) : IRequest<TokenResponse>;
    
}
