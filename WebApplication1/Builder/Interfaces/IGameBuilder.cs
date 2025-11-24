using WebApplication1.Models;

namespace WebApplication1.Builder.Interfaces
{
    public interface IGameBuilder
    {
        IGameBuilder CreateNew(string title, string description, EPlatform platform);
        IGameBuilder WithTechnicalDetails(DateTime? ReleaseDate, string? Language, string? Developer);
        IGameBuilder WithGenre(Genre Genre);
        IGameBuilder WithDefaultReview(Review review);
        Game Build();
    }
}
