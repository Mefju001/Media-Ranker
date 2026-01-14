using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Application.Common.DTO.Request;

namespace Application.Features.UserServices.ChangeDetails
{
    public record ChangeDetailsCommand(int userId, string name, string surname, string email) : IRequest<Unit>;
}
