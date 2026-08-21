using Application.Features.Common.Interfaces;
using Application.Features.Genres.Common;
using Application.Features.Medias.Games.Common;

namespace Application.Features.Medias.Games.Upsert
{
    public record UpsertCommand(
        Guid? Id,
        string Title,
        string Description,
        GenreRequest Genre,
        DateTime? ReleaseDate,
        string Language,
        string GameStatus,
        string Developer,
        string? Engine,
        int PegiRating,
        List<string> Platforms,
        bool SupportsCrossPlay) : ICommand<GameResponse>,ISendNotificationCommand;
}
