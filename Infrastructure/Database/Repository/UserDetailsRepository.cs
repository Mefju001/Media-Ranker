using Domain.Aggregate;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database.Repository
{
    public class UserDetailsRepository : Repository<UserDetails, Guid>, IUserDetailsRepository
    {
        public UserDetailsRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
