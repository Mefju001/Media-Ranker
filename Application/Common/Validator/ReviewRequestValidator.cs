using FluentValidation;
using Application.Common.DTO.Request;

namespace Application.Common.Validator
{
    public class ReviewRequestValidator:AbstractValidator<ReviewRequest>
    {
        public ReviewRequestValidator()
        {
            RuleFor(x => x.Rating).InclusiveBetween(1, 10).WithMessage("Rating must be between 1 and 10.");
            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Comment cannot be empty.")
                .MinimumLength(10).WithMessage("Comment must be at least 2 characters long.")
                .MaximumLength(250).WithMessage("Only 250 characters are allowed.");
        }
    }
}
