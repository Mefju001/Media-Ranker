namespace Application.Features.UserInteractions.Reviews.Upsert
{
    public record ReviewRequest(Guid MovieId, int Rating, string Comment);
}
