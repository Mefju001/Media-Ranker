using Domain.Value_Object;

namespace Domain.Aggregate;

public class Movie : Media
{
    public int DirectorId { get; private set; }
    public Duration Duration { get; private set; } = default!;
    public bool IsCinemaRelease { get; private set; }

    private Movie() { }


    public static Movie Create(
        string title, string desc, Language lang, ReleaseDate? date, int genre,
        int director, Duration duration, bool isCinema, int id = 0)
    {
        Validate(director);

        var movie = new Movie
        {
            Id = id,
            DirectorId = director,
            Duration = duration,
            IsCinemaRelease = isCinema
        };

        movie.SetBaseDetails(title, desc, lang, date, genre);

        return movie;
    }

    public void Update(
        string title, string desc, Language lang, ReleaseDate? date, int genre,
        int director, Duration duration, bool isCinema)
    {
        Validate(director);
        DirectorId = director;
        Duration = duration;
        IsCinemaRelease = isCinema;

        SetBaseDetails(title, desc, lang, date, genre);
    }

    public void UpdateCinemaStatus(bool isCinemaRelease) => IsCinemaRelease = isCinemaRelease;

    private static void Validate(int director)
    {
        if (director <= 0) throw new ArgumentException("Director ID must be greater than zero.");
    }
}