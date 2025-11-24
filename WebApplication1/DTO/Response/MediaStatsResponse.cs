namespace WebApplication1.DTO.Response
{
    public record MediaStatsResponse(int id, double? AverageRating, int? ReviewCount, DateTime? LastCalculated);
}
