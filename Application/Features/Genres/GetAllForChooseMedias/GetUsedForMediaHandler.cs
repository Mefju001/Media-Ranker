using Application.Features.Common.Interfaces;
using Application.Features.Genres.GetAll;
using Domain.Aggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Genres.GetAllForChooseMedias
{
    internal class GetUsedForMediaHandler<TMedia>: IRequestHandler<GetUsedForMediaQuery<TMedia>, List<GenreResponse>> where TMedia: Media
    {
        private readonly IAppDbContext appDbContext;
        public GetUsedForMediaHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<List<GenreResponse>> Handle(GetUsedForMediaQuery<TMedia> request, CancellationToken cancellationToken)
        {
            var genres = await appDbContext.Set<Genre>().AsNoTracking().Where(g =>
            appDbContext.Set<TMedia>().Any(m => m.GenreId == g.Id)).ToListAsync(cancellationToken);
            return genres.Select(g => GenreMapper.ToResponse(g)).ToList();
        }
    }
}
