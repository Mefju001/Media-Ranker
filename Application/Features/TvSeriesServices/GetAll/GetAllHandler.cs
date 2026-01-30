using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;

namespace Application.Features.TvSeriesServices.GetAll
{
    public class GetAllHandler : IRequestHandler<GetAllTvSeriesQuery, List<TvSeriesResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAllHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<List<TvSeriesResponse>> Handle(GetAllTvSeriesQuery request, CancellationToken cancellationToken)
        {
            var tvSeries = await unitOfWork.TvSeriesRepository.GetAll(cancellationToken);
            var tvSeriesResponses = tvSeries.Select(TvSeriesMapper.ToTvSeriesResponse).ToList();
            return (tvSeriesResponses);
        }
    }
}
