using Application.Features.Common.Interfaces;
using Application.Features.UserInteractions.Reviews.Common;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.UserInteractions.Reviews.GetAll
{
    internal class GetAllHandler : IRequestHandler<GetAllQuery, List<ReviewResponse>>
    {
        private readonly IAppDbContext appDbContext;
        public GetAllHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<List<ReviewResponse>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<Review>().AsNoTrackingWithIdentityResolution().AsSplitQuery().Select(x => ReviewMapper.ToResponse(x)).ToListAsync(cancellationToken);
        }
    }
}
