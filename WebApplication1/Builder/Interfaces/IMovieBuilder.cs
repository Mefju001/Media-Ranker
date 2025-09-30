using WebApplication1.Models;

namespace WebApplication1.Builder.Interfaces
{
    public interface IMovieBuilder
    {
        IMovieBuilder CreateNew(string title, string description);
        IMovieBuilder WithOptionals(DateTime? ReleaseDate,string? Language, TimeSpan? Duration,bool? IsCinemaRelease);
        IMovieBuilder WithDirector(Director director);
        IMovieBuilder WithGenre(Genre Genre);
        IMovieBuilder WithDefaultReview(Review review);
        Movie Build();
    }
}
