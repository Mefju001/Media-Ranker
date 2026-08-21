using domain = Domain.Aggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Aggregate;
using Application.Features.Common.Interfaces;
using Application.Features.Medias.TvSeries.Common;

namespace Application.Features.Medias.TvSeries.GetById
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
