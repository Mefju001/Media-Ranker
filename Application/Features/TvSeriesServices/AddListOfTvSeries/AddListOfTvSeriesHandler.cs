using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.TvSeriesServices.AddListOfTvSeries
{
    public class AddListOfTvSeriesHandler : IRequestHandler<AddListOfTvSeriesCommand, List<TvSeriesResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IReferenceDataService referenceDataService;
        public AddListOfTvSeriesHandler(IReferenceDataService referenceDataService, IUnitOfWork unitOfWork)
        {
            this.referenceDataService = referenceDataService;
            this.unitOfWork = unitOfWork;
        }
        public async Task<List<TvSeriesResponse>> Handle(AddListOfTvSeriesCommand requests, CancellationToken cancellationToken)
        {
            if (requests is null) throw new ArgumentNullException(nameof(requests));
            var genreNames = requests.tvSeries.Select(t => t.genre.name).Distinct().ToList();
            var genres = await referenceDataService.EnsureGenresExistAsync(genreNames);
            var tvSeries = requests.tvSeries.Select(tv=>
            {
                var genre = genres[tv.genre.name];
                return TvSeriesDomain.Create(tv.title,tv.description,tv.Language,tv.ReleaseDate,genre,tv.Seasons,tv.Episodes,tv.Network,tv.Status);
            }).ToList();
            await unitOfWork.TvSeriesRepository.AddListOfTvSeries(tvSeries);
            await unitOfWork.CompleteAsync();
            return tvSeries.Select(TvSeriesMapper.ToTvSeriesResponse).ToList();
        }
            
    }
}
