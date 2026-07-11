using domain = Domain.Aggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Aggregate;
using Application.Features.TvSeries.Common;
using Application.Features.Common.Interfaces;

namespace Application.Features.TvSeries.GetById
{
    internal class GetByIdHandler : IRequestHandler<GetByIdQuery, TvSeriesResponse?>
    {
        private readonly IAppDbContext appDbContext;

        public GetByIdHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<TvSeriesResponse?> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<domain.TvSeries>()
                .Where(x => x.Id == request.id)
                .Join(appDbContext.Set<Genre>(),
                    tvSeries => tvSeries.GenreId,
                    genre => genre.Id,
                    (tvSeries, genre) => new { TvSeries = tvSeries, Genre = genre })
                .Select(x => TvSeriesMapper.ToTvSeriesResponse(x.TvSeries, x.Genre))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
