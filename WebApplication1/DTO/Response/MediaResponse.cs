namespace WebApplication1.DTO.Response
{
    public record MediaResponse(
        string Title, 
        string Description, 
        GenreResponse Genre, 
        DirectorResponse Director,
        DateTime ReleaseDate,
        string? Language,
        List<ReviewResponse>?Reviews);
}
