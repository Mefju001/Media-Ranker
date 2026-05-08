using Application.Features.Common.Interfaces;
using Application.Features.Movies.Common;
using Domain.Aggregate;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.Movies.GetMovieById
{
    internal class GetByIdHandler : IRequestHandler<GetByIdQuery, MovieResponse?>
    {
        private readonly IAppDbContext appDbContext;

        public GetByIdHandler(IAppDbContext appDbContext)
        {

            this.appDbContext = appDbContext;
        }

        public async Task<MovieResponse?> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<Movie>()
                .AsNoTracking()
                .Where(m => m.Id == request.id)
                .Join(appDbContext.Set<Genre>(), m => m.GenreId, g => g.Id, (m, g) => new { Movie = m, Genre = g })
                .Join(appDbContext.Set<Director>(), mg => mg.Movie.DirectorId, d => d.Id, (mg, d) => new { mg.Movie, mg.Genre, Director = d })
                .Select(m => MovieMapper.ToMovieResponse(m.Movie, m.Genre, m.Director))
                .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException("Movie not found");
        }
    }
}
