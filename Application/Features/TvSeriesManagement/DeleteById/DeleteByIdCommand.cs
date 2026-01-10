using MediatR;

namespace WebApplication1.Application.Features.TvSeries.DeleteById
{
    public record DeleteByIdCommand(int id) : IRequest<bool>;
}
