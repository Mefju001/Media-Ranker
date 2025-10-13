using WebApplication1.Builder.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Builder
{
    public class GameBuilder : IGameBuilder
    {
        private Game game;
        public Game Build()
        {
            if (game.genre == null)
            {
                throw new InvalidOperationException("Game must havea Genre set.");
            }
            return game;
        }

        public IGameBuilder CreateNew(string title, string description, EPlatform platform)
        {
            game = new Game 
            { title = title, 
              description = description, 
              Platform = platform,
              Reviews = new List<Review>()
            };
            return this;
        }

        public IGameBuilder WithDefaultReview(Review review)
        {
            ArgumentNullException.ThrowIfNull(review, nameof(review));
            game.Reviews.Add(review);
            return this;
        }

        public IGameBuilder WithGenre(Genre Genre)
        {
            ArgumentNullException.ThrowIfNull(Genre, nameof(Genre));
            game.genre = Genre;
            return this;
        }

        public IGameBuilder WithTechnicalDetails(DateTime? ReleaseDate, string? Language, string? Developer)
        {
            game.ReleaseDate = ReleaseDate ?? DateTime.MinValue;
            game.Language = Language ?? string.Empty;
            game.Developer = Developer ?? string.Empty;
            return this;
        }
    }
}
