namespace WebApplication1.DTO.Request
{
    public record MovieRequest(
        string Title,
        string Description,
        GenreRequest Genre,
        DirectorRequest Director,
        DateTime? ReleaseDate,
        string Language,
        TimeSpan Duration,
        bool IsCinemaRelease
        );

}
