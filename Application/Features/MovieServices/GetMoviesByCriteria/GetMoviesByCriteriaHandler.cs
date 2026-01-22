using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Application.Features.MovieServices.GetMoviesByCriteria
{
    public class GetMoviesByCriteriaHandler : IRequestHandler<GetMoviesByCriteriaQuery, List<MovieResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetMoviesByCriteriaHandler(IUnitOfWork appDbContext)
        {
            unitOfWork = appDbContext;
        }

        public async Task<List<MovieResponse>> Handle(GetMoviesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.MovieRepository.AsQueryable();
            query = SortAndFilterService.ApplyFilters(query, request);
            query = SortAndFilterService.ApplySorting(query, request);
            var Response = await query.Select(MovieMapper.ToDto).ToListAsync(cancellationToken);
            return Response;
        }
    }
}
