using Application.Features.Games.Command;
using Application.Features.Movies.Common;
using Application.Features.TvSeries.Common;
using System.Text.Json.Serialization;

namespace Application.Features.Liked.Common
{
    [JsonDerivedType(typeof(MovieResponse), "movie")]
    [JsonDerivedType(typeof(GameResponse), "game")]
    [JsonDerivedType(typeof(TvSeriesResponse), "tvseries")]
    public record MediaResponse();
}
