using WebApplication1.Domain.Entities;
using WebApplication1.Domain.ValueObjects;

namespace WebApplication1.Domain.Interfaces
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
