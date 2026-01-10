namespace WebApplication1.Application.Common.DTO.Response
{
    public record MediaStatsResponse(int id, double? AverageRating, int? ReviewCount, DateTime? LastCalculated);
}
