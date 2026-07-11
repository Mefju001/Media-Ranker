namespace Application.Common.Interfaces
{
    public interface IMediaRepository<T> : IRepository<T, Guid> where T : Media
    {
    }
}
