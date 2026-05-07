namespace Application.Features.Reviews.Common
{
    public record ReviewResponse(Guid id, Guid MediaId, string username, int rating, string comment, DateTime CreatedAt, DateTime? LastModifiedAt);

}
