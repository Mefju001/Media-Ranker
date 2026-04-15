using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repository
{
    public class MediaRepository<T> : Repository<T, Guid>, IMediaRepository<T> where T : Media
    {
        public MediaRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
        public override async Task<T?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await appDbContext.Set<T>().Include(x => x.Reviews).FirstOrDefaultAsync(x => x.Id!.Equals(id), ct);
        }
    }
}
