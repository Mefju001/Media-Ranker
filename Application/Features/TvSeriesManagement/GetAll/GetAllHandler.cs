using MediatR;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Application.Mapper;

namespace WebApplication1.Application.Features.TvSeries.GetAll
{
    public class GetAllHandler : IRequestHandler<GetAllQuery, List<TvSeriesResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAllHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<List<TvSeriesResponse>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var tvSeries = await unitOfWork.TvSeries.GetAllAsync();
            var MovieResponse = tvSeries.Select(TvSeriesMapper.ToTvSeriesResponse).ToList();
            return (MovieResponse);
        }
    }
}
