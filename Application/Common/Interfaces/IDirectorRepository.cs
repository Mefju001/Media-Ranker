using Domain.Entity;


namespace Application.Common.Interfaces
{
    public interface IDirectorRepository
    {
        Task<Dictionary<int, DirectorDomain>> GetDirectorsDictionary();
        IQueryable<DirectorDomain> GetAllQueryable();
        Task<DirectorDomain?> Get(int id);
        Task<DirectorDomain?> FirstOrDefaultForNameAndSurnameAsync(string name, string surname);
        Task<DirectorDomain> AddAsync(DirectorDomain directorDomain);
        Task<List<DirectorDomain>> findByNames(List<(string, string)> names);
    }
}
