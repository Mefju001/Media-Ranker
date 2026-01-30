using Application.Common.Interfaces;
using Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ReviewServices.DeleteReviewAsync
{
    public class DeleteReviewAsync : IRequestHandler<DeleteReviewCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteReviewAsync(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.ReviewRepository.GetReviewByIdAsync(request.reviewId);
            if (review is null) throw new NotFoundException($"Review with id: {request.reviewId} does not exist");
            await _unitOfWork.ReviewRepository.DeleteAsync(review);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
