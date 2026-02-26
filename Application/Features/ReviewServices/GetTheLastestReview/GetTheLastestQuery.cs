using MediatR;

namespace Application.Features.ReviewServices.GetTheLastestReview
{
    public record GetTheLastestQuery : IRequest<List<string>>;
}
