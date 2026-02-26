using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using Domain.Value_Object;
using MediatR;

namespace Application.Features.TvSeriesServices.AddListOfTvSeries
{
    public class AddListOfTvSeriesHandler : IRequestHandler<AddListOfTvSeriesCommand, List<TvSeriesResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IReferenceDataService referenceDataService;
        private readonly ITvSeriesRepository tvSeriesRepository;
        public AddListOfTvSeriesHandler(IReferenceDataService referenceDataService, IUnitOfWork unitOfWork, ITvSeriesRepository tvSeriesRepository)
        {
            this.referenceDataService = referenceDataService;
            this.unitOfWork = unitOfWork;
            this.tvSeriesRepository = tvSeriesRepository;
        }
        public async Task<List<TvSeriesResponse>> Handle(AddListOfTvSeriesCommand requests, CancellationToken cancellationToken)
        {
            if (requests is null) throw new ArgumentNullException(nameof(requests));
            var genreNames = requests.tvSeries.Select(t => t.genre.name).Distinct().ToList();
            var genres = await referenceDataService.EnsureGenresExistAsync(genreNames);
            var tvSeries = requests.tvSeries.Select(tv =>
            {
                var genre = genres[tv.genre.name];
                return TvSeries.Create(tv.title, tv.description, new Language(tv.Language), new ReleaseDate(tv.ReleaseDate), genre.Id, tv.Seasons, tv.Episodes, tv.Network, tv.Status);
            }).ToList();
            await tvSeriesRepository.AddListOfTvSeries(tvSeries);
            await unitOfWork.CompleteAsync();
            return tvSeries.Select(m =>
            {
                var genreDomain = genres.Values.ToDictionary(g => g.Id);
                return TvSeriesMapper.ToTvSeriesResponse(m, genreDomain[m.GenreId]);
            }).ToList();
        }

    }
}
