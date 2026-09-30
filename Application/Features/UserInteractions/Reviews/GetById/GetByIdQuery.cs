using Application.Features.Common.Interfaces;
using Application.Features.UserInteractions.Reviews.Common;

namespace Application.Features.UserInteractions.Reviews.GetById
{
    public record GetByIdQuery(Guid reviewId) : IQuery<ReviewResponse?>;

}
