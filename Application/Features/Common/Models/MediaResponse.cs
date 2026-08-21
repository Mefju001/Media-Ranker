using Application.Features.Genres.GetAll;
using Application.Features.Medias.Games.Common;
using Application.Features.Medias.Movies.Common;
using Application.Features.Medias.TvSeries.Common;
using System.Text.Json.Serialization;

namespace Application.Features.Common.Models
{
    [JsonDerivedType(typeof(MovieResponse), "movie")]
    [JsonDerivedType(typeof(GameResponse), "game")]
    [JsonDerivedType(typeof(TvSeriesResponse), "tvseries")]
    public abstract record MediaResponse(Guid id, string Title, string Description, GenreResponse GenreResponse, DateTime ReleaseDate, string Language, MediaStatsResponse MediaStatsResponse);
}
