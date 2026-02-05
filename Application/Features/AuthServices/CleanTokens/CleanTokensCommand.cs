using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AuthServices.CleanTokens
{
    public class CleanTokensCommand:IRequest<Unit>
    {
    }
}
