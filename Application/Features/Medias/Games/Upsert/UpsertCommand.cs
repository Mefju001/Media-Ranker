using Application.Features.Common.Interfaces;
using Application.Features.Genres.Common;
using Application.Features.Medias.Games.Common;
using Domain.Enums;

namespace Application.Features.Medias.Games.Upsert
{
    public record UpsertCommand(
        Guid? Id,
        string Title,
        string Description,
        GenreRequest Genre,
        DateTime? ReleaseDate,
        string Language,
        EGameStatus GameStatus,
        string Developer,
        string? Engine,
        int PegiRating,
        List<EPlatform> Platforms,
        bool SupportsCrossPlay) : ICommand<GameResponse>,ISendNotificationCommand;
}
