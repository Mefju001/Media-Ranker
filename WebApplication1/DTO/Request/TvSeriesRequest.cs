namespace WebApplication1.DTO.Request
{
    public record TvSeriesRequest(
        string title,
        string description,
        GenreRequest genre,
        DirectorRequest director,
        DateTime ReleaseDate,
        string Language,
        int Seasons,
        int Episodes,
        string Network,
        string Status
        )
    {
    }
}
