using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ReviewServices.GetByIdReview
{
    public class GetByIdReviewHandler : IRequestHandler<GetByIdReviewQuery, ReviewResponse?>
    {
        private readonly IAppDbContext appDbContext;
        public GetByIdReviewHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<ReviewResponse?> Handle(GetByIdReviewQuery query, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<Review>().AsNoTrackingWithIdentityResolution().AsSplitQuery().Where(x => x.Id == query.reviewId).Select(x => ReviewMapper.ToResponse(x)).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
