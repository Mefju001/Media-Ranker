using MediatR;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Application.Mapper;
using WebApplication1.Domain.Interfaces;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Application.Features.TvSeries.TvSeriesUpsert
{
    public class TvSeriesUpsertHandler : IRequestHandler<UpsertTvSeriesCommand, TvSeriesResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator _mediator;
        private readonly ITvSeriesBuilder tvSeriesBuilder;
        private readonly IReferenceDataService referenceDataService;

        public TvSeriesUpsertHandler(IUnitOfWork unitOfWork, IReferenceDataService referenceDataService, ITvSeriesBuilder tvSeriesBuilder, IMediator mediator)
        {
            this.unitOfWork = unitOfWork;
            this._mediator = mediator;
            this.tvSeriesBuilder = tvSeriesBuilder;
            this.referenceDataService = referenceDataService;
        }

        public async Task<TvSeriesResponse> Handle(UpsertTvSeriesCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                var genre = await referenceDataService.GetOrCreateGenreAsync(request.genre);
                WebApplication1.Domain.Entities.TvSeries? tvSeries;
                if (request.id is not null)
                {
                    tvSeries = await unitOfWork.TvSeries
                            .FirstOrDefaultAsync(m => m.Id == request.id);
                    if (tvSeries is not null)
                    {
                        TvSeriesMapper.UpdateEntity(tvSeries, request, genre);
                    }
                }
                else
                {
                    tvSeries = tvSeriesBuilder.CreateNew(request.title, request.description)
                        .WithGenre(genre)
                        .WithMetadata
                        (request.Seasons,
                        request.Episodes,
                        request.Network,
                        request.Status)
                        .Build();
                    await unitOfWork.TvSeries.AddAsync(tvSeries);
                }
                await unitOfWork.CompleteAsync();
                if (tvSeries is null) throw new ArgumentNullException(nameof(tvSeries));
                var response = TvSeriesMapper.ToTvSeriesResponse(tvSeries);
                await transaction.CommitAsync();
                return response;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
