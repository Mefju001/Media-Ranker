using Application.Common.Interfaces;
using Application.Features.MoviesManagement.GetMoviesByCriteria;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Mapper;
using WebApplication1.Domain.Entities;


namespace WebApplication1.Application.Features.Movies.GetMoviesByCriteria
{
    public class GetMoviesByCriteriaHandler : IRequestHandler<GetMoviesByCriteriaQuery, List<MovieResponse>>
    {
        private readonly IAppDbContext context;
        private readonly ISorterContext<Movie> sorterContext;
        private readonly IMovieBuildPredicate movieBuildPredicate;
        private readonly IMovieFilter movieFilter;

        public GetMoviesByCriteriaHandler(ISorterContext<Movie> sorterContext, IAppDbContext appDbContext, IMovieBuildPredicate movieBuildPredicate, IMovieFilter movieFilter)
        {
            this.sorterContext = sorterContext;
            this.context = appDbContext;
            this.movieBuildPredicate = movieBuildPredicate;
            this.movieFilter = movieFilter;
        }


        public async Task<List<MovieResponse>> Handle(GetMoviesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var query = context.Movies
                .Include(m => m.genre)
                .Include(m => m.director)
                .Include(m => m.Stats)
                .AsNoTracking()
                .AsQueryable();
            //IQueryable<Movie> query = queryServices.StartQuery();
            var predicate = movieBuildPredicate.BuildPredicate(request);
            query = movieFilter.Filter(query, predicate);
            if (!string.IsNullOrEmpty(request.SortByField) && request.SortByField.Contains('|'))
            {
                var fields = request.SortByField.Split('|');
                if (fields.Length == 2 && bool.TryParse(fields[1], out bool IsDescending))
                {
                    request.IsDescending = IsDescending;
                    request.SortByField = fields[0];
                    query = sorterContext.Sort(query, request.SortByField, request.IsDescending);
                }
                else throw new Exception("Mismatch");
            }
            var movies = await query.ToListAsync(cancellationToken);
            var Response = movies.Select(m => MovieMapper.ToMovieResponse(m)).ToList();
            return Response;
        }
    }
}
