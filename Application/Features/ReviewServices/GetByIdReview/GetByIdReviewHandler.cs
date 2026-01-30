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
        private readonly IUnitOfWork _unitOfWork;
        public GetByIdReviewHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ReviewResponse?>Handle(GetByIdReviewQuery query, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.ReviewRepository.GetReviewByIdAsync(query.reviewId);
            if (review == null)
            {
                return null;
            }
            return ReviewMapper.ToResponse(review);
        }
    }
}
