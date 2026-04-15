using Domain.Aggregate;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repository
{
    public class UserDetailsRepository : Repository<UserDetails, Guid>, IUserDetailsRepository
    {
        public UserDetailsRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }
        public override Task<UserDetails?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return appDbContext.Set<UserDetails>().Include(u => u.LikedMedias).FirstOrDefaultAsync(u => u.Id == id, ct);
        }
    }
}
