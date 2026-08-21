using Application.Features.Common.Interfaces;
using Application.Features.UserInteractions.Reviews.Common;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.UserInteractions.Reviews.GetById
{
    internal class GetByIdHandler : IRequestHandler<GetByIdQuery, ReviewResponse?>
    {
        private readonly IAppDbContext appDbContext;
        public GetByIdHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<ReviewResponse?> Handle(GetByIdQuery query, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<Review>().AsNoTrackingWithIdentityResolution().AsSplitQuery().Where(x => x.Id == query.reviewId).Select(x => ReviewMapper.ToResponse(x)).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
