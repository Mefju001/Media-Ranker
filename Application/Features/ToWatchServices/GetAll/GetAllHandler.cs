

using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ToWatchServices.GetAll
{
    public class GetAllHandler:IRequestHandler<GetAllQuery, List<ToWatchResponse>>
    {
        private IAppDbContext appDbContext;
        public GetAllHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<List<ToWatchResponse>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.ToWatchlists.Where(x => x.UserId == request.UserId)
                .Select(x => new ToWatchResponse
                (
                    x.Id,
                    x.UserId,
                    x.MediaId,
                    x.LikedDate
                )).ToListAsync(cancellationToken);
        }
    }
}
