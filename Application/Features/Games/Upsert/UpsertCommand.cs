using Application.Features.Common.Interfaces;
using Application.Features.Games.Command;
using Application.Features.Genres.Common;

namespace Application.Features.Games.Upsert
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
