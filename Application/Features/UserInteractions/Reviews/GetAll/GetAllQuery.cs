using Application.Features.Common.Interfaces;
using Application.Features.UserInteractions.Reviews.Common;

namespace Application.Features.UserInteractions.Reviews.GetAll
{
    public record GetAllQuery : IQuery<List<ReviewResponse>>;
}
