namespace WebApplication1.Application.Common.Interfaces
{
    public interface ISortingStrategy<T> where T : class
    {
        string Key { get; }
        public IQueryable<T> ApplySort(IQueryable<T> query, bool isDescending);
    }
}
