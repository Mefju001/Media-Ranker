using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Domain.Aggregate;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.MovieServices.GetMovieById
{
    public class GetMovieByIdHandler : IRequestHandler<GetMovieByIdQuery, MovieResponse?>
    {
        private readonly IAppDbContext appDbContext;

        public GetMovieByIdHandler(IAppDbContext appDbContext)
        {
            
            this.appDbContext = appDbContext;
        }

        public async Task<MovieResponse?> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<Movie>()
                .AsNoTracking()
                .Where(m => m.Id == request.id)
                .Join(appDbContext.Set<Genre>(), m => m.GenreId, g => g.Id, (m, g) => new { Movie = m, Genre = g })
                .Join(appDbContext.Set<Director>(), mg => mg.Movie.DirectorId, d => d.Id, (mg, d) => new { mg.Movie, mg.Genre, Director = d })
                .Select(m=> 
                new MovieResponse(
                    m.Movie.Id,
                    m.Movie.Title, 
                    m.Movie.Description,
                    new GenreResponse(m.Genre.Id,m.Genre.Name),
                    new DirectorResponse(m.Director.Id,m.Director.Name,m.Director.Surname),
                    m.Movie.ReleaseDate,
                    m.Movie.Language,
                    m.Movie.Reviews.Select(r=>new ReviewResponse(r.Id,r.MediaId,r.Username,r.Rating,r.Comment,r.AuditInfo.CreatedAt,r.AuditInfo.UpdatedAt)).ToList(),
                    new MediaStatsResponse(m.Movie.Stats.AverageRating,m.Movie.Stats.ReviewCount,m.Movie.Stats.LastCalculated),
                    m.Movie.Duration,
                    m.Movie.IsCinemaRelease))
                .FirstOrDefaultAsync(cancellationToken)??throw new NotFoundException("Movie not found");
        }
    }
}
