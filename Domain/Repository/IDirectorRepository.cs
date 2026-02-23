using Domain.Entity;


namespace Application.Common.Interfaces
{
    public interface IDirectorRepository
    {
        Task<Dictionary<int, Director>> GetDirectorsDictionary();
        IQueryable<Director> GetAllQueryable();
        Task<Director?> Get(int id);
        Task<Director?> FirstOrDefaultForNameAndSurnameAsync(string name, string surname);
        Task<Director> AddAsync(Director directorDomain);
        Task<List<Director>> findByNames(List<(string, string)> names);
    }
}
