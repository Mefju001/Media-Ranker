namespace Application.Common.DTO.Request
{
    public record ReviewRequest(Guid MovieId, int Rating, string Comment);
}
