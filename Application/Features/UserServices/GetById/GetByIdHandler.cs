using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.UserServices.GetById
{
    public class GetByIdHandler : IRequestHandler<GetByIdQuery, Unit>
    {
        public Task<Unit> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
