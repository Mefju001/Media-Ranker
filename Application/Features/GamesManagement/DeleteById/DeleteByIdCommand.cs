using MediatR;

namespace WebApplication1.Application.Features.Games.DeleteById
{
    public record DeleteByIdCommand(int id) : IRequest<bool>;
}
