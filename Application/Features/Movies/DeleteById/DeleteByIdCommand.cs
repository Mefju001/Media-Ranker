using Application.Features.Common.Interfaces;

namespace Application.Features.Movies.DeleteById
{
    public record DeleteByIdCommand(Guid id) : ICommand<bool>;
}
