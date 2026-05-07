using Application.Features.Common.Interfaces;
using Application.Features.Games.Command;
using Application.Features.Genres.GetAll;
using Application.Features.Movies.Common;
using Application.Features.Reviews.Common;
using Application.Features.TvSeries.Common;
using System.Text.Json.Serialization;

namespace Application.Features.Common.DTO.Response
{
    [JsonDerivedType(typeof(MovieResponse), "movie")]
    [JsonDerivedType(typeof(GameResponse), "game")]
    [JsonDerivedType(typeof(TvSeriesResponse), "tvSeries")]
    public record MediaResponse(
        Guid id,
        string Title,
        string Description,
        GenreResponse Genre,
        DateTime ReleaseDate,
        string? Language,
        List<ReviewResponse>? Reviews) : IResponse;
}
