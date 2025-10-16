namespace WebApplication1.Strategy.Interfaces
{
    public interface ISortingStrategy<T>where T : class
    {
        public IQueryable<T>ApplySort(IQueryable<T>query);
    }
}
