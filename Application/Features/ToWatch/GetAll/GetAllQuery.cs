using Application.Features.ToWatch.GetAll;
using MediatR;

namespace Application.Features.ToWatch.GetAll
{
    public record GetAllQuery(Guid UserId) : IRequest<List<ToWatchResponse>>;
}
