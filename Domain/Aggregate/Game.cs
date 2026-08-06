using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Value_Object;

namespace Domain.Aggregate;

public class Game : Media, MediaInfo
{
    public EGameStatus Status { get; private set; }  = EGameStatus.Announced;
    public GameDetails? Details { get; private set; } = default!;
    public PegiRating PegiRating {  get; private set; } = PegiRating.Pegi3;
    public GamePlatforms Platforms { get; private set; } = default!;
    public bool SupportsCrossPlay { get; private set; } = false;

    private Game() { }

    public static Game Create(
        string title, string desc, string lang, ReleaseDate? date, Guid genre,
        GameDetails gameDetails, int pegiRating, List<EPlatform> platforms, EGameStatus status, bool supportsCrossPlay, Guid? id = null)
    {
        var game = new Game
        {
            Id = id ?? Guid.NewGuid()
        };
        game.SetBaseDetails(title, desc, lang, date, genre);
        game.Platforms = GamePlatforms.FromPlatforms(platforms);
        game.Details = gameDetails ?? throw new DomainException(nameof(gameDetails));
        game.PegiRating = PegiRating.FromValue(pegiRating);
        game.Status = status;
        game.SupportsCrossPlay = supportsCrossPlay;
        return game;
    }

    public void Update(
        string title, string desc, string lang, ReleaseDate? date, Guid genre,
        GameDetails gameDetails, int pegiRating, List<EPlatform> platforms, EGameStatus status, bool supportsCrossPlay)
    {
        if (Status == EGameStatus.Cancelled)
            throw new DomainException("Cannot update a cancelled game.");

        Details = gameDetails ?? throw new DomainException(nameof(gameDetails));
        PegiRating = PegiRating.FromValue(pegiRating);
        Status = status;
        SupportsCrossPlay = supportsCrossPlay;

        Platforms = GamePlatforms.FromPlatforms(platforms);

        SetBaseDetails(title, desc, lang, date, genre);
    }

}