using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ReviewServices.GetAllReviewsAsync
{
    public class GetAllReviewsHandler : IRequestHandler<GetAllReviewsQuery, List<ReviewResponse>>
    {
        private readonly IAppDbContext appDbContext;
        public GetAllReviewsHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<List<ReviewResponse>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<Review>().AsNoTrackingWithIdentityResolution().AsSplitQuery().Select(x => ReviewMapper.ToResponse(x)).ToListAsync(cancellationToken);
        }
    }
}
