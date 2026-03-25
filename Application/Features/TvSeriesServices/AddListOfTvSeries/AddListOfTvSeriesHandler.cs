using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Application.Notification;
using Domain.Entity;
using Domain.Exceptions;
using Domain.Value_Object;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.TvSeriesServices.AddListOfTvSeries
{
    public class AddListOfTvSeriesHandler : IRequestHandler<AddListOfTvSeriesCommand, List<TvSeriesResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IReferenceDataService referenceDataService;
        private readonly ITvSeriesRepository tvSeriesRepository;
        private readonly ILogger<AddListOfTvSeriesHandler> logger;
        private readonly IMediator mediator;
        public AddListOfTvSeriesHandler(IMediator mediator, IReferenceDataService referenceDataService, ILogger<AddListOfTvSeriesHandler> logger, IUnitOfWork unitOfWork, ITvSeriesRepository tvSeriesRepository)
        {
            this.mediator = mediator;
            this.logger = logger;
            this.referenceDataService = referenceDataService;
            this.unitOfWork = unitOfWork;
            this.tvSeriesRepository = tvSeriesRepository;
        }
        public async Task<List<TvSeriesResponse>> Handle(AddListOfTvSeriesCommand requests, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(requests);
            if (requests.tvSeries.Count > 500)
                throw new BadRequestException("The package is too large. Maximum 500 series at a time.");
            logger.LogInformation("Received request to add a list of TV series. TV series count: {TvSeriesCount}", requests.tvSeries.Count);
            var genreNames = requests.tvSeries.Select(t => t.genre.name).Distinct().ToList();
            var genres = await referenceDataService.EnsureGenresExistAsync(genreNames, cancellationToken);
            logger.LogInformation("Ensured genres exist for the provided TV series. Genre count: {GenreCount}", genres.Count);
            var tvSeries = requests.tvSeries.Select(tv =>
            {
                var genre = genres[tv.genre.name];
                return TvSeries.Create(tv.title, tv.description, new Language(tv.Language), new ReleaseDate(tv.ReleaseDate), genre.Id, tv.Seasons, tv.Episodes, tv.Network, tv.Status);
            }).ToList();
            await tvSeriesRepository.AddListOfTvSeries(tvSeries, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            logger.LogInformation("Added list of TV series to the repository. TV series count: {TvSeriesCount}", tvSeries.Count);
            await mediator.Publish(new LogNotification("Information", "Nowa lista seriali została dodana.", nameof(AddListOfTvSeriesHandler)));
            var genresById = genres.Values.ToDictionary(g => g.Id);
            return tvSeries.Select(tv =>
            {
                return TvSeriesMapper.ToTvSeriesResponse(tv, genresById[tv.GenreId]);
            }).ToList();
        }
    }
}
