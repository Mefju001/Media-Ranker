using WebApplication1.Domain.Entities;

namespace WebApplication1.Domain.Interfaces
{
    public interface IMovieBuilder
    {
        IMovieBuilder CreateNew(string title, string description);
        IMovieBuilder WithTechnicalDetails(
            TimeSpan? duration,
            string? language,
            bool? isCinemaRelease,
            DateTime? releaseDate);
        IMovieBuilder WithDirector(Director director);
        IMovieBuilder WithGenre(Genre Genre);
        IMovieBuilder WithDefaultReview(Review review);
        Movie Build();
    }
}
