using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.TvSeriesServices.DeleteById
{
    public record DeleteByIdCommand(Guid id) : ICommand<Unit>;
}
