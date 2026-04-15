using Domain.Enums;
using Domain.Value_Object;

namespace Domain.Aggregate;

public class Game : Media
{
    public string Developer { get; private set; } = default!;
    private List<EPlatform> platforms = new();
    public IReadOnlyCollection<EPlatform> Platforms => platforms.AsReadOnly();
    private Game() { }
    public static Game Create(
        string title, string desc, Language lang, ReleaseDate? date, Guid genre,
        string developer, List<EPlatform> platforms, Guid? id = null)
    {
        Validate(developer);
        ValidatePlatforms(platforms);
        var game = new Game
        {
            Id = id ?? Guid.NewGuid(),
            Developer = developer,
            platforms = new List<EPlatform>(platforms)
        };

        game.SetBaseDetails(title, desc, lang, date, genre);
        return game;
    }

    public void Update(
        string title, string desc, Language lang, ReleaseDate? date, Guid genre,
        string developer, List<EPlatform> platforms)
    {
        Validate(developer);
        ValidatePlatforms(platforms);
        Developer = developer;
        this.platforms = new List<EPlatform>(platforms);

        SetBaseDetails(title, desc, lang, date, genre);
    }

    private static void Validate(string developer)
    {
        if (string.IsNullOrWhiteSpace(developer))
            throw new ArgumentException("Developer cannot be null or empty.");
    }
    private static void ValidatePlatforms(List<EPlatform> platforms)
    {
        if (platforms == null || !platforms.Any())
            throw new ArgumentException("Game must have at least one platform.");
    }
}