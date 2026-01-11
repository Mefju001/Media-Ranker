using MediatR;
using WebApplication1.Application.Common.DTO.Request;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Domain.ValueObjects;

namespace WebApplication1.Application.Features.Games.MovieUpsert
{
    public record UpsertGameCommand(
        int? id,
        string Title,
        string Description,
        GenreRequest Genre,
        DateTime? ReleaseDate,
        string Language,
        string? Developer,
        EPlatform Platform) : IRequest<GameResponse>;
}
