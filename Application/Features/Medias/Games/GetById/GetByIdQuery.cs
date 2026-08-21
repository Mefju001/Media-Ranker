using Application.Features.Common.Interfaces;
using Application.Features.Medias.Games.Common;

namespace Application.Features.Medias.Games.GetById
{
    public record GetByIdQuery(Guid id) : IQuery<GameResponse?>;

}
