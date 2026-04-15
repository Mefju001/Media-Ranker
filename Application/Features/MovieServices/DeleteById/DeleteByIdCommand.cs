using Application.Common.Interfaces;

namespace Application.Features.MovieServices.DeleteById
{
    public record DeleteByIdCommand(Guid id) : ICommand<bool>;
}
