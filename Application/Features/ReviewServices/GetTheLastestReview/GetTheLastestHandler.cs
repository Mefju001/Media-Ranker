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
        private readonly IUnitOfWork _unitOfWork;
        public GetTheLastestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<string>> Handle(GetTheLastestQuery request, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.ReviewRepository.GetTheLastestReviewAsync(cancellationToken);
            return review;
        }
    }
}
