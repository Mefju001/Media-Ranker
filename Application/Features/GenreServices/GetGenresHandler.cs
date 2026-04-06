using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Aggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.GenreServices
{
    public class GetGenresHandler : IRequestHandler<GetGenresQuery, List<GenreResponse>>
    {
        private readonly IAppDbContext appDbContext;
        public GetGenresHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<List<GenreResponse>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<Genre>().AsNoTracking().Select(g => GenreMapper.ToResponse(g)).ToListAsync(cancellationToken);
        }
    }
}
