using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using MediatR;

namespace Application.Features.TvSeriesServices.TvSeriesUpsert
{
    public class TvSeriesUpsertHandler : IRequestHandler<UpsertTvSeriesCommand, TvSeriesResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator _mediator;
        private readonly IReferenceDataService referenceDataService;

        public TvSeriesUpsertHandler(IUnitOfWork unitOfWork, IReferenceDataService referenceDataService, IMediator mediator)
        {
            this.unitOfWork = unitOfWork;
            this._mediator = mediator;
            this.referenceDataService = referenceDataService;
        }

        public async Task<TvSeriesResponse> Handle(UpsertTvSeriesCommand request, CancellationToken cancellationToken)
        {
            var genre = await referenceDataService.GetOrCreateGenreAsync(request.genre);
            TvSeriesDomain? tvSeries;
            if (request.id is not null)
            {
                tvSeries = await unitOfWork.TvSeriesRepository.GetTvSeriesById(request.id.Value);
                if (tvSeries is not null)
                {
                    TvSeriesDomain.Update(
                        request.title,
                        request.description,
                        request.Language,
                        request.ReleaseDate,
                        genre.Id,
                        request.Seasons,
                        request.Episodes,
                        request.Network,
                        request.Status,
                        tvSeries);
                }
            }
            else
            {
                tvSeries = TvSeriesDomain.Create(
                        request.title,
                        request.description,
                        request.Language,
                        request.ReleaseDate,
                        genre.Id,
                        request.Seasons,
                        request.Episodes,
                        request.Network,
                        request.Status);
                await unitOfWork.TvSeriesRepository.AddTvSeriesAsync(tvSeries);
            }
            await unitOfWork.CompleteAsync();
            if (tvSeries is null) throw new ArgumentNullException(nameof(tvSeries));
            var response = TvSeriesMapper.ToTvSeriesResponse(tvSeries,genre);
            return response;
        }
    }
}
