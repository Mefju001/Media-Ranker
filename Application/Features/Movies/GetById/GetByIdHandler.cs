using Application.Common.DTO.Response;
using Application.Features.Common.Interfaces;
using Application.Features.Genre.GetAll;
using Application.Features.Movies.Common;
using Domain.Aggregate;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.Movies.GetMovieById
{
    public class GetByIdHandler : IRequestHandler<GetByIdQuery, MovieResponse?>
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
                .Join(appDbContext.Set<Domain.Aggregate.Genre>(), m => m.GenreId, g => g.Id, (m, g) => new { Movie = m, Genre = g })
                .Join(appDbContext.Set<Director>(), mg => mg.Movie.DirectorId, d => d.Id, (mg, d) => new { mg.Movie, mg.Genre, Director = d })
                .Select(m =>
                new MovieResponse(
                    m.Movie.Id,
                    m.Movie.Title,
                    m.Movie.Description,
                    new GenreResponse(m.Genre.Id, m.Genre.Name),
                    new DirectorResponse(m.Director.Id, m.Director.fullname.Name, m.Director.fullname.Surname),
                    m.Movie.ReleaseDate,
                    m.Movie.Language,
                    m.Movie.Reviews.Select(r => new ReviewResponse(r.Id, r.MediaId, r.Username, r.Rating, r.Comment, r.AuditInfo.CreatedAt, r.AuditInfo.UpdatedAt)).ToList(),
                    new MediaStatsResponse(m.Movie.Stats.AverageRating, m.Movie.Stats.ReviewCount, m.Movie.Stats.LastCalculated),
                    m.Movie.Duration,
                    m.Movie.IsCinemaRelease))
                .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException("Movie not found");
        }
    }
}
