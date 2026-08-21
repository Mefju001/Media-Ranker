using Application.Features.Common.Models;
using Application.Features.Medias.Games.Common;
using Application.Features.Medias.Movies.Common;
using Application.Features.Medias.TvSeries.Common;
using Domain.Aggregate;

namespace Application.Features.Common.Mapping
{
    public static class MediaMappingExtensions
    {
        public static MediaResponse ToResponse(this Media media, Genre genre,
        Director? director = null) => media switch
        {
            Movie movie => movie.ToMovieResponse(genre, director),
            Game game => game.ToGameResponse(genre),
            Domain.Aggregate.TvSeries tvSeries => tvSeries.ToTvSeriesResponse(genre),
            _ => throw new ArgumentException($"Nieobsługiwany typ multimediów: {media.GetType().Name}")
        };
        private static MovieResponse ToMovieResponse(this Movie movie, Genre genreDomain, Director director)
        {
            return MovieMapper.ToMovieResponse(movie, genreDomain, director);
        }

        private static GameResponse ToGameResponse(this Game gameDomain, Genre genreDomain)
        {
            return GameMapper.ToGameResponse(gameDomain, genreDomain);
        }

        private static TvSeriesResponse ToTvSeriesResponse(this Domain.Aggregate.TvSeries tvSeriesDomain, Genre genreDomain)
        {
            return TvSeriesMapper.ToTvSeriesResponse(tvSeriesDomain, genreDomain);
        }
    }
}
