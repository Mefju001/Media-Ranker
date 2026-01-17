using MediatR;

namespace Application.Features.MovieServices.DeleteById
{
    public record DeleteByIdCommand(int id) : IRequest<bool>;
}
