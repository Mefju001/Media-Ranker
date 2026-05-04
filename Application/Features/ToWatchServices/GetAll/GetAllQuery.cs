using MediatR;

namespace Application.Features.ToWatchServices.GetAll
{
    public record GetAllQuery(Guid UserId) : IRequest<List<ToWatchResponse>>;
}
