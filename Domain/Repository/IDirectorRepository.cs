using Domain.Aggregate;

namespace Application.Common.Interfaces
{
    public interface IDirectorRepository:IRepository<Director,int>
    {
        Task<Dictionary<int, Director>> GetByIds(List<int> ids, CancellationToken cancellationToken);
        Task<Dictionary<int, Director>> GetDirectorsDictionary(CancellationToken cancellationToken);
        Task<Director?> FirstOrDefaultForNameAndSurnameAsync(string name, string surname, CancellationToken cancellationToken);
        Task<List<Director>> findByNames(List<(string, string)> names, CancellationToken cancellationToken);
    }
}
