using Application.Common.Interfaces;
using Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ReviewServices.GetAllReviewsAsync
{
    public class GetAllReviewsHandler : IRequestHandler<GetAllReviewsQuery, List<ReviewDomain>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllReviewsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<ReviewDomain>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _unitOfWork.ReviewRepository.GetAllReviewsAsync(cancellationToken);
            return reviews;
        }
    }
}
