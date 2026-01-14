using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.LikedServices.Delete
{
    public record DeleteLikedCommand(int userId, int mediaId) : IRequest<bool>;
}
