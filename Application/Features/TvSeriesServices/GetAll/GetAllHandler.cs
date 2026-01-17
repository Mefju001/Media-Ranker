using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
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
            throw new NotImplementedException();
            /*TvSeriesDomain tvSeries = await unitOfWork.TvSeries.GetAllAsync();
            var MovieResponse = tvSeries.Select(TvSeriesMapper.ToTvSeriesResponse).ToList();
            return (MovieResponse);*/
        }
    }
}
