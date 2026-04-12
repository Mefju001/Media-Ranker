using Domain.Aggregate;
using Domain.Repository;

namespace Infrastructure.Database.Repository
{
    public class UserDetailsRepository : Repository<UserDetails, Guid>, IUserDetailsRepository
    {
        public UserDetailsRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }
    }
}
