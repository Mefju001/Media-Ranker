using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.MovieServices.DeleteById
{
    public record DeleteByIdCommand(int id) : ICommand<bool>;
}
