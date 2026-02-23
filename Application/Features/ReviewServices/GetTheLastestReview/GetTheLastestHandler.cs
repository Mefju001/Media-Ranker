using Application.Common.Interfaces;
using Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ReviewServices.GetTheLastestReview
{
    public class GetTheLastestHandler:IRequestHandler<GetTheLastestQuery,List<string>>
    {
        private readonly IReviewRepository reviewRepository;
        public GetTheLastestHandler(IReviewRepository reviewRepository)
        {
            this.reviewRepository = reviewRepository;
        }
        public async Task<List<string>> Handle(GetTheLastestQuery request, CancellationToken cancellationToken)
        {
            var review = await reviewRepository.GetTheLastestReviewAsync(cancellationToken);
            return review;
        }
    }
}
