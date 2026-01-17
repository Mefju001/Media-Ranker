using MediatR;

namespace Application.Features.TvSeriesServices.DeleteById
{
    public record DeleteByIdCommand(int id) : IRequest<bool>;
}
