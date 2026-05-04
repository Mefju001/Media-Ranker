using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ToWatchServices
{
    public record ToWatchResponse(Guid Id, Guid UserId, Guid MediaId, DateTime LikedDate);
}
