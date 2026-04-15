using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.GamesServices.DeleteById
{
    public record DeleteByIdCommand(Guid id) : ICommand<Unit>;
}
