using Application.Features.Common.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ToWatch.GetAll
{
    internal class GetAllHandler:IRequestHandler<GetAllQuery, List<ToWatchResponse>>
    {
        private IAppDbContext appDbContext;
        public GetAllHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<List<ToWatchResponse>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.UserInteractions.Where(x => x.UserId == request.UserId && x.TypeInteractions == ETypeInteractions.WANT_TO_WATCH)
                .Select(x => new ToWatchResponse
                (
                    x.Id,
                    x.UserId,
                    x.MediaId,
                    x.InteractionDate
                )).ToListAsync(cancellationToken);
        }
    }
}
