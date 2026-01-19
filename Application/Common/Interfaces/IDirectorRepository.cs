using Domain.Entity;


namespace Application.Common.Interfaces
{
    public interface IDirectorRepository
    {
        DirectorDomain? Get(int id);
        Task<DirectorDomain?> FirstOrDefaultForNameAndSurnameAsync(string name, string surname);
        Task<DirectorDomain> AddAsync(DirectorDomain directorDomain);
    }
}
