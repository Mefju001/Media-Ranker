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
        private readonly IUnitOfWork unitOfWork;
        private readonly IReviewRepository reviewRepository;
        public DeleteReviewAsync(IUnitOfWork unitOfWork, IReviewRepository reviewRepository)
        {
            this.unitOfWork = unitOfWork;
            this.reviewRepository = reviewRepository;
        }

        public async Task<Unit> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await reviewRepository.GetReviewByIdAsync(request.reviewId);
            if (review is null) throw new NotFoundException($"Review with id: {request.reviewId} does not exist");
            await reviewRepository.DeleteAsync(review);
            await unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
