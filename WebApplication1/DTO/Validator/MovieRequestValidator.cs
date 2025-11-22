using FluentValidation;
using WebApplication1.DTO.Request;

namespace WebApplication1.DTO.Validator
{
    public class MovieRequestValidator : AbstractValidator<MovieRequest>
    {
        public MovieRequestValidator()
        {
            RuleFor(Request => Request.Title)
                .NotEmpty().WithMessage("Title is must have")
                .MaximumLength(250).WithMessage("Only 250 characters are allowed.");
            RuleFor(Request => Request.Description)
                .NotEmpty().WithMessage("Description should have text.")
                .MaximumLength(250).WithMessage("Only 250 characters are allowed.");
            RuleFor(Request => Request.Genre.name)
                .NotEmpty().WithMessage("Genre name should have text.")
                .MaximumLength(200).WithMessage("Only 200 characters are allowed.");
            RuleFor(Request => Request.Director.Name)
                .NotEmpty().WithMessage("Director should have name.");
            RuleFor(Request => Request.Director.Surname)
                .NotEmpty().WithMessage("Director should have surname.");
            RuleFor(Request => Request.ReleaseDate)
                .NotEmpty()
                .LessThanOrEqualTo(DateTime.UtcNow)
                .When(Request => Request.IsCinemaRelease);
            RuleFor(Request => Request.ReleaseDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("The release date of the announcement must be a future date.")
                .When(Request => Request.IsCinemaRelease == false && Request.ReleaseDate.HasValue);
            RuleFor(Request => Request.Language)
                .NotEmpty().WithMessage("Language should have text.")
                .MaximumLength(200).WithMessage("Only 200 characters are allowed.");
            RuleFor(Request => Request.Duration)
                .NotEmpty()
                .GreaterThan(TimeSpan.Zero);
        }
    }
}
