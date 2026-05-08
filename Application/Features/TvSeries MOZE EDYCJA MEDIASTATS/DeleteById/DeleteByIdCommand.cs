using Application.Features.Common.Interfaces;
using MediatR;

namespace Application.Features.TvSeries.DeleteById
{
    public record DeleteByIdCommand(Guid id) : ICommand<Unit>;
}
