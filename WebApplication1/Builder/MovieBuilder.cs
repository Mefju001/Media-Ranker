using WebApplication1.Builder.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Builder
{
    public class MovieBuilder : IMovieBuilder
    {
        private Movie _movie;

        public Movie Build()
        {
            if (_movie.director == null || _movie.genre == null)
            {
                throw new InvalidOperationException("Movie must have both a Director and a Genre set.");
            }
            return _movie;
        }

        public IMovieBuilder CreateNew(string title,string description)
        {
            _movie = new Movie
            {
                title = title,
                description = description,
                Reviews = new List<Review>()
            };
            return this;
        }

        public IMovieBuilder WithDefaultReview(Review review)
        {
            ArgumentNullException.ThrowIfNull(review);
            _movie.Reviews.Add(review);
            return this;
        }

        public IMovieBuilder WithDirector(Director director)
        {
            ArgumentNullException.ThrowIfNull(director);
            _movie.director = director;
            return this;
        }

        public IMovieBuilder WithGenre(Genre genre)
        {
            ArgumentNullException.ThrowIfNull(genre);
            _movie.genre = genre;
            return this;
        }

        public IMovieBuilder WithOptionals(DateTime? ReleaseDate, string? Language, TimeSpan? Duration, bool? IsCinemaRelease)
        {
            _movie.ReleaseDate = ReleaseDate ?? DateTime.MinValue;
            _movie.Language = Language ?? string.Empty;
            _movie.Duration = Duration ?? TimeSpan.Zero;
            _movie.IsCinemaRelease = IsCinemaRelease ?? false;

            return this;

        }
    }
}
