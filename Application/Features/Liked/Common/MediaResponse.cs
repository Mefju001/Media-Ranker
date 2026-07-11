using Application.Features.Games.Command;
using Application.Features.Genres.GetAll;
using Application.Features.Movies.Common;
using Application.Features.TvSeries.Common;
using System.Text.Json.Serialization;

namespace Application.Features.Liked.Common
{
    [JsonDerivedType(typeof(MovieResponse), "movie")]
    [JsonDerivedType(typeof(GameResponse), "game")]
    [JsonDerivedType(typeof(TvSeriesResponse), "tvseries")]
    public abstract record MediaResponse(Guid id, string Title, string Description, GenreResponse GenreResponse, DateTime ReleaseDate, string Language, MediaStatsResponse MediaStatsResponse);
}
