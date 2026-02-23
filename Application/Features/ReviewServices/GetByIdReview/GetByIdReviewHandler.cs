using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Features.ReviewServices.GetTheLastestReview;
using Application.Mapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ReviewServices.GetByIdReview
{
    public class GetByIdReviewHandler : IRequestHandler<GetByIdReviewQuery, ReviewResponse?>
    {
        private readonly IReviewRepository reviewRepository;
        public GetByIdReviewHandler(IReviewRepository reviewRepository)
        {
            this.reviewRepository = reviewRepository;
        }
        public async Task<ReviewResponse?>Handle(GetByIdReviewQuery query, CancellationToken cancellationToken)
        {
            var review = await reviewRepository.GetReviewByIdAsync(query.reviewId);
            if (review == null)
            {
                return null;
            }
            return ReviewMapper.ToResponse(review);
        }
    }
}
