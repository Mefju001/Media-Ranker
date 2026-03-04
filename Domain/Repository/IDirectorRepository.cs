using Domain.Entity;


namespace Application.Common.Interfaces
{
    public interface IDirectorRepository
    {
        Task<Dictionary<int,Director>>GetByIds(List<int> ids, CancellationToken cancellationToken);
        Task<Dictionary<int, Director>> GetDirectorsDictionary(CancellationToken cancellationToken);
        IQueryable<Director> GetAllQueryable();
        Task<Director?> Get(int id, CancellationToken cancellationToken);
        Task<Director?> FirstOrDefaultForNameAndSurnameAsync(string name, string surname, CancellationToken cancellationToken);
        Task<Director> AddAsync(Director directorDomain,CancellationToken cancellationToken);
        Task<List<Director>> findByNames(List<(string, string)> names, CancellationToken cancellationToken);
    }
}
