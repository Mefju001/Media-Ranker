using Domain.Aggregate;

namespace Application.Common.Interfaces
{
    public interface IDirectorRepository : IRepository<Director, Guid>
    {
        Task<Dictionary<Guid, Director>> GetByIds(List<Guid> ids, CancellationToken cancellationToken);
        Task<Dictionary<Guid, Director>> GetDirectorsDictionary(CancellationToken cancellationToken);
        Task<Director?> FirstOrDefaultForNameAndSurnameAsync(string name, string surname, CancellationToken cancellationToken);
        Task<List<Director>> findByNames(List<(string, string)> names, CancellationToken cancellationToken);
    }
}
