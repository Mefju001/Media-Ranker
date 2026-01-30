using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ReviewServices.DeleteReviewAsync
{
    public record DeleteReviewCommand(int reviewId):IRequest<Unit>;
}
