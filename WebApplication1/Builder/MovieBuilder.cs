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

        public IMovieBuilder CreateNew(string title, string description)
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

        public IMovieBuilder WithTechnicalDetails(TimeSpan? duration, string? language, bool? isCinemaRelease, DateTime? releaseDate)
        {
            _movie.ReleaseDate = releaseDate ?? DateTime.MinValue;
            _movie.Language = language ?? string.Empty;
            _movie.Duration = duration ?? TimeSpan.Zero;
            _movie.IsCinemaRelease = isCinemaRelease ?? false;

            return this;

        }
    }
}
