using Application.Common.DTO.Request;
using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Domain.Enums;

namespace Application.Features.GamesServices.GameUpsert
{
    public record UpsertGameCommand(
        Guid? id,
        string Title,
        string Description,
        GenreRequest Genre,
        DateTime? ReleaseDate,
        string Language,
        string? Developer,
        List<EPlatform> Platforms) : ICommand<GameResponse>;
}
