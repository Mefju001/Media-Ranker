using Application.Features.Common.Interfaces;
using Application.Features.Games.Command;
using Application.Features.Genres.Common;
using Domain.Enums;

namespace Application.Features.Games.Upsert
{
    public record UpsertCommand(
        Guid? id,
        string Title,
        string Description,
        GenreRequest Genre,
        DateTime? ReleaseDate,
        string Language,
        string? Developer,
        List<EPlatform> Platforms) : ICommand<GameResponse>;
}
