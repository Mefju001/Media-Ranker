namespace Domain.Repository
{
    public interface IMediaRepository<T> : IRepository<T, Guid> where T : Media
    {
    }
}
