using MediatR;

namespace Application.Features.GamesServices.DeleteById
{
    public record DeleteByIdCommand(int id) : IRequest<bool>;
}
