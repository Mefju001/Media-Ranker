using Application.Features.Common.Interfaces;
using Application.Features.Reviews.Common;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Reviews.GetById
{
    public class GetByIdReviewHandler : IRequestHandler<GetByIdQuery, ReviewResponse?>
    {
        private readonly IAppDbContext appDbContext;
        public GetByIdReviewHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<ReviewResponse?> Handle(GetByIdQuery query, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<Review>().AsNoTrackingWithIdentityResolution().AsSplitQuery().Where(x => x.Id == query.reviewId).Select(x => ReviewMapper.ToResponse(x)).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
