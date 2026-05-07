namespace Application.Features.Common.DTO.Response
{
    public record MediaStatsResponse(double? AverageRating, int? ReviewCount, DateTime? LastCalculated);
}
