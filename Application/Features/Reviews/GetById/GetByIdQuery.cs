using Application.Features.Common.Interfaces;
using Application.Features.Reviews.Common;

namespace Application.Features.Reviews.GetById
{
    public record GetByIdQuery(Guid reviewId) : IQuery<ReviewResponse?>;

}
