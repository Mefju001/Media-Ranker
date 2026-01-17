using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.MovieServices.GetMoviesByCriteria
{
    public class GetMoviesByCriteriaHandler : IRequestHandler<GetMoviesByCriteriaQuery, List<MovieResponse>>
    {
        private readonly IAppDbContext context;
        private readonly ISorterContext<MovieDomain> sorterContext;
        private readonly IMovieBuildPredicate movieBuildPredicate;
        private readonly IMovieFilter movieFilter;

        public GetMoviesByCriteriaHandler(ISorterContext<MovieDomain> sorterContext, IAppDbContext appDbContext, IMovieBuildPredicate movieBuildPredicate, IMovieFilter movieFilter)
        {
            this.sorterContext = sorterContext;
            this.context = appDbContext;
            this.movieBuildPredicate = movieBuildPredicate;
            this.movieFilter = movieFilter;
        }


        public async Task<List<MovieResponse>> Handle(GetMoviesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var query = context.Movies
                //.Include(m => m.)
                //.Include(m => m.director)
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
            var Response = movies.Select(m => MovieMapper.ToMovieResponse(m,GenreDomain.Create(""),DirectorDomain.Create("",""))).ToList();
            return Response;
        }
    }
}
