using MediatR;
using Microsoft.EntityFrameworkCore.Query.Internal;
using WebApplication1.DTO.Response;
using WebApplication1.Models;
using WebApplication1.QueryHandler.Query;

namespace WebApplication1.QueryHandler
{
    public class MovieQueryHandler:IRequestHandler<MoviesQuery,List<MovieResponse>>
    {
        private readonly QueryService<Movie> queryService;

        public Task<List<MovieResponse>> Handle(MoviesQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
