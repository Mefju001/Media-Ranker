using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using Domain.Value_Object;
using MediatR;

namespace Application.Features.TvSeriesServices.TvSeriesUpsert
{
    public class TvSeriesUpsertHandler : IRequestHandler<UpsertTvSeriesCommand, TvSeriesResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator _mediator;
        private readonly IReferenceDataService referenceDataService;
        private readonly ITvSeriesRepository tvSeriesRepository;

        public TvSeriesUpsertHandler(IUnitOfWork unitOfWork, IReferenceDataService referenceDataService, IMediator mediator, ITvSeriesRepository tvSeriesRepository)
        {
            this.unitOfWork = unitOfWork;
            this._mediator = mediator;
            this.referenceDataService = referenceDataService;
            this.tvSeriesRepository = tvSeriesRepository;
        }

        public async Task<TvSeriesResponse> Handle(UpsertTvSeriesCommand request, CancellationToken cancellationToken)
        {
            var genre = await referenceDataService.GetOrCreateGenreAsync(request.genre);
            TvSeries? tvSeries;
            if (request.id is not null)
            {
                tvSeries = await tvSeriesRepository.GetTvSeriesById(request.id.Value);
                if (tvSeries is not null)
                {
                    tvSeries.Update(
                        request.title,
                        request.description,
                        new Language(request.Language),
                        new ReleaseDate(request.ReleaseDate),
                        genre.Id,
                        request.Seasons,
                        request.Episodes,
                        request.Network,
                        request.Status
                        );
                }
            }
            else
            {
                tvSeries = TvSeries.Create(
                        request.title,
                        request.description,
                        new Language(request.Language),
                        new ReleaseDate(request.ReleaseDate),
                        genre.Id,
                        request.Seasons,
                        request.Episodes,
                        request.Network,
                        request.Status);
                tvSeries = await tvSeriesRepository.AddTvSeriesAsync(tvSeries);
            }
            await unitOfWork.CompleteAsync();
            if (tvSeries is null) throw new ArgumentNullException(nameof(tvSeries));
            var response = TvSeriesMapper.ToTvSeriesResponse(tvSeries, genre);
            return response;
        }
    }
}
