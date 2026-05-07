using Application.Features.Common.Interfaces;
using Application.Features.Reviews.Common;

namespace Application.Features.Reviews.GetAll
{
    public record GetAllQuery : IQuery<List<ReviewResponse>>;
}
