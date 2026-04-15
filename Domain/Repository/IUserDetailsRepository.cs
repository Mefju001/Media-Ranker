using Application.Common.Interfaces;
using Domain.Aggregate;

namespace Domain.Repository
{
    public interface IUserDetailsRepository : IRepository<UserDetails, Guid>
    {
    }
}
