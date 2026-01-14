using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AuthServices.Signup
{
    public class SignUpHandler : IRequestHandler<SignUpCommand, SignUpResponse>
    {
        public SignUpHandler()
        {
        }

        public Task<SignUpResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
