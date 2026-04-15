using Application.Common.Interfaces;

namespace Application.Features.ReviewServices.GetTheLastestReview
{
    public record GetTheLastestQuery : IQuery<List<string>>;
}
