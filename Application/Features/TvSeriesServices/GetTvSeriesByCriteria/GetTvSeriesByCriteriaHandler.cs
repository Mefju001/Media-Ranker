using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.TvSeriesServices.GetTvSeriesByCriteria
{
    public class GetTvSeriesByCriteriaHandler : IRequestHandler<GetTvSeriesByCriteriaQuery, List<TvSeriesResponse>>
    {
        private readonly IUnitOfWork unitOfWork;


        public GetTvSeriesByCriteriaHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public async Task<List<TvSeriesResponse>> Handle(GetTvSeriesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var query = await unitOfWork.TvSeriesRepository.AsQueryable();
            query = SortAndFilterService.ApplyFilters(query,request);
            query = SortAndFilterService.ApplySorting(query, request);
            var Response = await query.Select(TvSeriesMapper.ToDto).ToListAsync(cancellationToken);
            return Response;
        }


    }
}
