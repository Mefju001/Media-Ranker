using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.ReviewServices.GetTheLastestReview
{
    public record GetTheLastestQuery : IQuery<List<string>>;
}
