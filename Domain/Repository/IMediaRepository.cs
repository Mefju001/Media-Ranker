using Domain.Aggregate;
using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IMediaRepository<T>:IRepository<T,int>where T : Media
    {
    }
}
