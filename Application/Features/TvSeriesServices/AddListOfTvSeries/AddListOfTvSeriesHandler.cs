using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Application.Notification;
using Domain.Aggregate;
using Domain.Exceptions;
using Domain.Value_Object;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.TvSeriesServices.AddListOfTvSeries
{
    //maybe add better response with info about which games were added and which not, and why.

    public class AddListOfTvSeriesHandler : IRequestHandler<AddListOfTvSeriesCommand, List<TvSeriesResponse>>
    {
        private readonly IGenreHelperService genreHelperService;
        private readonly IMediaRepository<TvSeries> mediaRepository;
        private readonly IMediator mediator;
        public AddListOfTvSeriesHandler(IMediator mediator, IGenreHelperService genreHelperService, IMediaRepository<TvSeries> mediaRepository)
        {
            this.mediator = mediator;
            this.genreHelperService = genreHelperService;
            this.mediaRepository = mediaRepository;
        }
        public async Task<List<TvSeriesResponse>> Handle(AddListOfTvSeriesCommand requests, CancellationToken cancellationToken)
        {
            if (requests.tvSeries == null || !requests.tvSeries.Any()) return [];
            if (requests.tvSeries.Count > 500)
                throw new BadRequestException("The package is too large. Maximum 500 series at a time.");
            var genreNames = requests.tvSeries.Select(t => t.genre.name).Distinct().ToList();
            var genres = await genreHelperService.EnsureGenresExistAsync(genreNames, cancellationToken);
            var tvSeries = requests.tvSeries.Select(tv =>
            {
                var genre = genres[tv.genre.name];
                return TvSeries.Create(tv.title, tv.description, new Language(tv.Language), new ReleaseDate(tv.ReleaseDate), genre.Id, tv.Seasons, tv.Episodes, tv.Network, tv.Status);
            }).ToList();
            await mediaRepository.AddRangeAsync(tvSeries, cancellationToken);
            await mediator.Publish(new LogNotification("Information", "Nowa lista seriali została dodana.", nameof(AddListOfTvSeriesHandler)));
            var genresById = genres.Values.ToDictionary(g => g.Id);
            return tvSeries.Select(tv => TvSeriesMapper.ToTvSeriesResponse(tv, genresById[tv.GenreId])).ToList();
        }
    }
}
