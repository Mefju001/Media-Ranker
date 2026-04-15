using System.Text.Json.Serialization;

namespace Application.Common.DTO.Response
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
