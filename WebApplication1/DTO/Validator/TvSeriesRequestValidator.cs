using FluentValidation;
using WebApplication1.DTO.Request;

namespace WebApplication1.DTO.Validator
{
    public class TvSeriesRequestValidator:AbstractValidator<TvSeriesRequest>
    {
        public TvSeriesRequestValidator()
        {
            RuleFor(Request => Request.title)
                    .NotEmpty().WithMessage("Title is must have")
                    .MaximumLength(250).WithMessage("Only 250 characters are allowed.");
            RuleFor(Request => Request.description)
                    .NotEmpty().WithMessage("Description should have text.")
                    .MaximumLength(250).WithMessage("Only 250 characters are allowed.");
            RuleFor(Request => Request.genre.name)
                    .NotEmpty().WithMessage("Genre name should have text.")
                    .MaximumLength(200).WithMessage("Only 200 characters are allowed.");
           //edycja
            RuleFor(Request => Request.ReleaseDate)
                    .NotEmpty()
                    .LessThanOrEqualTo(DateTime.UtcNow)
                    .When(Request => Request.Status == "Finished" || Request.Status == "Released");
            RuleFor(Request => Request.Language)
                    .NotEmpty().WithMessage("Language name should have text.")
                    .MaximumLength(250).WithMessage("Only 250 characters are allowed.");
            RuleFor(Request => Request.Seasons)
                .NotEmpty()
                .GreaterThanOrEqualTo(1).WithMessage("Seasons must start with one.");
            RuleFor(Request=>Request.Episodes)
                .NotEmpty()
                .GreaterThanOrEqualTo(1).WithMessage("Episodes must start with one.");
            RuleFor(Request => Request.Network)
                .NotEmpty()
                .MaximumLength(250).WithMessage("Only 250 characters are allowed.");
        }
    }
}
