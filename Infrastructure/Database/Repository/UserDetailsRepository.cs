using Application.Features.Common.Interfaces;
using Domain.Aggregate;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repository
{
    public class UserDetailsRepository(IAppDbContext appDbContext) : Repository<UserDetails, Guid>(appDbContext), IUserDetailsRepository
    {
        public override Task<UserDetails?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return dbSet.Include(u => u.UserInteractions).FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        public async Task<string?> GetUsernameById(Guid id, CancellationToken cancellationToken)
        {
            return await dbSet.Where(u => u.Id == id).Select(u => u.Username).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
