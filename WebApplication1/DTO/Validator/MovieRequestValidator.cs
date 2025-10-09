using FluentValidation;
using WebApplication1.DTO.Request;

namespace WebApplication1.DTO.Validator
{
    public class MovieRequestValidator:AbstractValidator<MovieRequest>
    {
        public MovieRequestValidator()
        {
            RuleFor(Request => Request.Title)
                .NotEmpty().WithMessage("Title is must have")
                .MaximumLength (250).WithMessage("Only 250 characters are allowed.");
            RuleFor(Request => Request.Description)
                .NotEmpty().WithMessage("Description should have text.")
                .MaximumLength(250).WithMessage("Only 250 characters are allowed.");
            RuleFor(Request => Request.)
        }
    }
}
