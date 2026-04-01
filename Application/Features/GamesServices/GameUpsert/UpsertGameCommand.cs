using Application.Common.DTO.Request;
using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Domain.Enums;
using MediatR;

namespace Application.Features.GamesServices.GameUpsert
{
    public record UpsertGameCommand(
        int? id,
        string Title,
        string Description,
        GenreRequest Genre,
        DateTime? ReleaseDate,
        string Language,
        string? Developer,
        EPlatform Platform) : ICommand<GameResponse>;
}
