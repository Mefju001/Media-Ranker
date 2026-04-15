using Domain.Value_Object;

namespace Domain.Aggregate;

public class Movie : Media
{
    public Guid DirectorId { get; private set; }
    public Duration Duration { get; private set; } = default!;
    public bool IsCinemaRelease { get; private set; }

    private Movie() { }


    public static Movie Create(
        string title, string desc, Language lang, ReleaseDate? date, Guid genre,
        Guid director, Duration duration, bool isCinema, Guid? id = null)
    {
        var movie = new Movie
        {
            Id = id ?? Guid.NewGuid(),
            DirectorId = director,
            Duration = duration,
            IsCinemaRelease = isCinema
        };

        movie.SetBaseDetails(title, desc, lang, date, genre);

        return movie;
    }

    public void Update(
        string title, string desc, Language lang, ReleaseDate? date, Guid genre,
        Guid director, Duration duration, bool isCinema)
    {
        DirectorId = director;
        Duration = duration;
        IsCinemaRelease = isCinema;

        SetBaseDetails(title, desc, lang, date, genre);
    }

    public void UpdateCinemaStatus(bool isCinemaRelease) => IsCinemaRelease = isCinemaRelease;


}