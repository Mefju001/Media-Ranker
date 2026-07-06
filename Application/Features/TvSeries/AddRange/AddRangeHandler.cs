using Application.Common.Interfaces;
using Application.Features.Genres.GenreManager;
using Application.Features.TvSeries.Common;
using Domain.Exceptions;
using Domain.Value_Object;
using MediatR;
using domain = Domain.Aggregate;

namespace Application.Features.TvSeries.AddRange
{
    //maybe add better response with info about which games were added and which not, and why.

    internal class AddRangeHandler : IRequestHandler<AddRangeCommand, List<TvSeriesResponse>>
    {
        private readonly IGenreManager genreHelperService;
        private readonly IMediaRepository<domain.TvSeries> mediaRepository;
        public AddRangeHandler( IGenreManager genreHelperService, IMediaRepository<domain.TvSeries> mediaRepository)
        {
            this.genreHelperService = genreHelperService;
            this.mediaRepository = mediaRepository;
        }
        public async Task<List<TvSeriesResponse>> Handle(AddRangeCommand requests, CancellationToken cancellationToken)
        {
            if (requests.tvSeries == null || !requests.tvSeries.Any()) return [];
            if (requests.tvSeries.Count > 500)
                throw new BadRequestException("The package is too large. Maximum 500 series at a time.");
            var genreNames = requests.tvSeries.Select(t => t.genre.name).Distinct().ToList();
            var listOfGenres = await genreHelperService.GetOrCreateBatchAsync(genreNames, cancellationToken);
            var genres = listOfGenres.ToDictionary(g => g.Name, g => g);
            var tvSeries = requests.tvSeries.Select(tv =>
            {
                var genre = genres[tv.genre.name];
                return domain.TvSeries.Create(tv.title, tv.description, tv.Language, new ReleaseDate(tv.ReleaseDate), genre.id, new SeasonDetails(tv.Seasons,tv.Episodes), tv.Network, tv.Status);
            }).ToList();
            await mediaRepository.AddRangeAsync(tvSeries, cancellationToken);
            var genresById = genres.Values.ToDictionary(g => g.id);
            return tvSeries.Select(tv => TvSeriesMapper.ToTvSeriesResponse(tv, genresById[tv.GenreId])).ToList();
        }
    }
}
