using Application.Features.Common.Interfaces;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Reviews.GetTheLastestTitle
{
    internal class GetTheLastestTitleHandler : IRequestHandler<GetTheLastestTitleQuery, List<string>>
    {
        private readonly IAppDbContext appDbContext;
        public GetTheLastestTitleHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<List<string>> Handle(GetTheLastestTitleQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<Review>().AsNoTracking()
                .Join(appDbContext.Set<Media>(), r => r.MediaId, m => m.Id, (r, m) => new { Review = r, Media = m })
                .OrderByDescending(x => x.Review.AuditInfo.CreatedAt)
                .Take(10)
                .Select(x => x.Media.Title)
                .ToListAsync(cancellationToken);
        }
    }
}
