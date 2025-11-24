namespace WebApplication1.DTO.Response
{
    public record MediaResponse(
        int id,
        string Title,
        string Description,
        GenreResponse Genre,
        DateTime ReleaseDate,
        string? Language,
        List<ReviewResponse>? Reviews);
}
