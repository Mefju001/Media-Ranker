namespace WebApplication1.DTO.Response
{
    public record MovieAVGResponse
        (string Title,
        string Description,
        GenreResponse Genre,
        DirectorResponse Director,
        DateTime ReleaseDate,
        string? Language,
        List<ReviewResponse>? Reviews,

        TimeSpan Duration,
        bool IsCinemaRelease,
        double Avarage
        ) : MediaResponse(Title, Description, Genre, ReleaseDate, Language, Reviews)
    {
        public MovieAVGResponse(MovieResponse movie, double avg) : this(
            movie.Title, movie.Description, movie.Genre, movie.Director, movie.ReleaseDate,
            movie.Language, movie.Reviews, movie.Duration, movie.IsCinemaRelease, avg
            )
        { }
    }
}
