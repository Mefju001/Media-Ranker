using Application.Features.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Genres.GetAll
{
    public class GetAllHandler : IRequestHandler<GetAllQuery, List<GenreResponse>>
    {
        private readonly IAppDbContext appDbContext;
        public GetAllHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<List<GenreResponse>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<Domain.Aggregate.Genre>().AsNoTracking().Select(g => GenreMapper.ToResponse(g)).ToListAsync(cancellationToken);
        }
    }
}
