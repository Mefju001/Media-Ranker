using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IGenreRepository
    {
        GenreDomain? Get(int id);
        Task<GenreDomain?> FirstOrDefaultForNameAsync(string name);
        Task<GenreDomain> AddAsync(GenreDomain genre);
        Task<List<GenreDomain>>GetByNamesAsync(List<string> names);
    }
}
