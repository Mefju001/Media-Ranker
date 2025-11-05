namespace WebApplication1.DTO.Response
{
    public record MediaStatsResponse(double? AverageRating, int? ReviewCount, DateTime? LastCalculated);
}
