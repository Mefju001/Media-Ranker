using Domain.Enums;
using FluentValidation;

namespace Application.Features.Movies.Common
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
            When(request => Enum.TryParse<EMovieStatus>(request.Status, out var status) && status == EMovieStatus.Announced, () =>
            {
                RuleFor(request => request.ReleaseDate)
                    .NotEmpty().WithMessage("Release date is required for announced movies.")
                    .GreaterThan(DateTime.UtcNow).WithMessage("The release date of the announcement must be a future date.");
            });
            When(request => Enum.TryParse<EMovieStatus>(request.Status, out var status) && status != EMovieStatus.Announced, () =>
            {
                RuleFor(request => request.ReleaseDate)
                    .NotEmpty().WithMessage("Release date is required.")
                    .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Release date cannot be in the future for this status.");
            });
            RuleFor(Request => Request.Language)
                .NotEmpty().WithMessage("Language should have text.")
                .MaximumLength(200).WithMessage("Only 200 characters are allowed.");
            RuleFor(Request => Request.Duration)
                .NotEmpty()
                .GreaterThan(TimeSpan.Zero);
        }
    }
}
