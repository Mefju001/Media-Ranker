using MediatR;

namespace WebApplication1.Application.Features.Movies.DeleteById
{
    public record DeleteByIdCommand(int id) : IRequest<bool>;
}
