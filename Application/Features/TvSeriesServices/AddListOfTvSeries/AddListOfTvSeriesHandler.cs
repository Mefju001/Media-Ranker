using MediatR;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Application.Mapper;
using WebApplication1.Domain.Interfaces;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Application.Features.TvSeries.AddListOfTvSeries
{
    public class AddListOfTvSeriesHandler : IRequestHandler<AddListOfTvSeriesCommand, List<TvSeriesResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITvSeriesBuilder tvSeriesBuilder;
        private readonly IReferenceDataService referenceDataService;
        public AddListOfTvSeriesHandler(IReferenceDataService referenceDataService, IUnitOfWork unitOfWork, ITvSeriesBuilder builder)
        {
            this.referenceDataService = referenceDataService;
            this.tvSeriesBuilder = builder;
            this.unitOfWork = unitOfWork;
        }
        public async Task<List<TvSeriesResponse>> Handle(AddListOfTvSeriesCommand requests, CancellationToken cancellationToken)
        {
            if (requests is null) throw new ArgumentNullException(nameof(requests));
            await using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                List<WebApplication1.Domain.Entities.TvSeries> listTvSeries = new List<WebApplication1.Domain.Entities.TvSeries>();
                foreach (var request in requests.requests)
                {
                    var genre = await referenceDataService.GetOrCreateGenreAsync(request.genre);
                    var tvSeries = tvSeriesBuilder.CreateNew(request.title, request.description)
                        .WithGenre(genre)
                        .WithMetadata
                        (request.Seasons,
                        request.Episodes,
                        request.Network,
                        request.Status)
                        .Build();
                    listTvSeries.Add(tvSeries);
                }
                await unitOfWork.TvSeries.AddRangeAsync(listTvSeries);
                await unitOfWork.CompleteAsync();
                var listOfResponses = listTvSeries.Select(TvSeriesMapper.ToTvSeriesResponse).ToList();
                await transaction.CommitAsync();
                return listOfResponses;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
