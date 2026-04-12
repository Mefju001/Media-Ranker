using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.TvSeriesServices.DeleteById
{
    public record DeleteByIdCommand(int id) : ICommand<Unit>;
}
