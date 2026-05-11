using Application.Features.Common.Interfaces;
using Application.Features.Games.Command;

namespace Application.Features.Games.GetById
{
    public record GetByIdQuery(Guid id) : IQuery<GameResponse?>;

}
