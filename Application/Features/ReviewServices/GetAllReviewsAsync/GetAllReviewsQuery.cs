using Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ReviewServices.GetAllReviewsAsync
{
    public record GetAllReviewsQuery : IRequest<List<Review>>;
}
