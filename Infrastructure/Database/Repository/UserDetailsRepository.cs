using Application.Features.Common.Interfaces;
using Domain.Aggregate;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repository
{
    public class UserDetailsRepository : Repository<UserDetails, Guid>, IUserDetailsRepository
    {
        public UserDetailsRepository(IAppDbContext appDbContext) : base(appDbContext)
        {

        }
        public override Task<UserDetails?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return appDbContext.Set<UserDetails>().Include(u => u.UserInteractions).FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        public async Task<string?> GetUsernameById(Guid id, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<UserDetails>().Where(u => u.Id == id).Select(u => u.Username).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
