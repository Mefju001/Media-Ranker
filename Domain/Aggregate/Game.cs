using Domain.Enums;
using Domain.Interfaces;
using Domain.Value_Object;

namespace Domain.Aggregate;

public class Game : Media
{
    public string Developer { get; private set; } = default!;
    public EPlatform Platform { get; private set; }

    private Game() { }
    public static Game Create(
        string title, string desc, Language lang, ReleaseDate? date, int genre,
        string developer, EPlatform platform, int id = 0)
    {
        Validate(developer);

        var game = new Game
        {
            Id = id,
            Developer = developer,
            Platform = platform
        };

        game.SetBaseDetails(title, desc, lang, date, genre);
        return game;
    }

    public void Update(
        string title, string desc, Language lang, ReleaseDate? date, int genre,
        string developer, EPlatform platform)
    {
        Validate(developer);
        Developer = developer;
        Platform = platform;

        SetBaseDetails(title, desc, lang, date, genre);
    }

    private static void Validate(string developer)
    {
        if (string.IsNullOrWhiteSpace(developer))
            throw new ArgumentException("Developer cannot be null or empty.");
    }
}