namespace Application.Features.ToWatch.GetAll
{
    public record ToWatchResponse(Guid Id, Guid UserId, Guid MediaId, DateTime LikedDate);
}
