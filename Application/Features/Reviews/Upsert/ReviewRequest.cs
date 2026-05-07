namespace Application.Features.Reviews.Upsert
{
    public record ReviewRequest(Guid MovieId, int Rating, string Comment);
}
