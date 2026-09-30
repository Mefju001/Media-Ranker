using Application.Features.Common.Interfaces;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repository
{
    public class MediaRepository<T>(IAppDbContext appDbContext) : Repository<T, Guid>(appDbContext), IMediaRepository<T> where T : Media
    {
        public override async Task<T?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await dbSet.Include(x => x.Reviews).FirstOrDefaultAsync(x => x.Id!.Equals(id), ct);
        }
    }
}
