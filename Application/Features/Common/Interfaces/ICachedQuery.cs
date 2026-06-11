namespace Application.Features.Common.Interfaces
{
    public interface ICachedQuery
    {
        string CacheKey { get; }
        TimeSpan? Expiration { get; }
    }
}
