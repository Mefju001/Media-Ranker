using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Aggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TvSeriesServices.GetTvSeriesById
{
    public class GetTvSeriesByIdHandler : IRequestHandler<GetTvSeriesByIdQuery, TvSeriesResponse?>
    {
        private readonly IAppDbContext appDbContext;

        public GetTvSeriesByIdHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<TvSeriesResponse?> Handle(GetTvSeriesByIdQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<TvSeries>()
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
