using FluentValidation;
using WebApplication1.DTO.Request;
using WebApplication1.Models;

namespace WebApplication1.DTO.Validator
{
    public class GameRequestValidator:AbstractValidator<GameRequest>
    {

        public GameRequestValidator()
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
            RuleFor(Request => Request.ReleaseDate)
                    .NotEmpty();
            RuleFor(request => request.ReleaseDate)
                    .LessThanOrEqualTo(DateTime.UtcNow)
                    .WithMessage("“If the game has been released, the release date must be a past or present date.”")
                    .When(request => request.ReleaseDate.HasValue && request.ReleaseDate.Value <= DateTime.UtcNow);
            RuleFor(request => request.ReleaseDate)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("The release date of the announcement must be a future date.")
                .When(request => request.ReleaseDate.HasValue && request.ReleaseDate.Value > DateTime.UtcNow);
            RuleFor(Request => Request.Language)
                    .NotEmpty().WithMessage("Language name should have text.")
                    .MaximumLength(250).WithMessage("Only 250 characters are allowed.");
            RuleFor(Request => Request.Platform)
                .NotEmpty()
                .IsInEnum().WithMessage("The value entered is incorrect.")
                .NotEqual(EPlatform.Unknown)
                .WithMessage("You must enter the correct value, not the default value.");
            RuleFor(Request => Request.Developer)
                .NotEmpty()
                .MaximumLength(250).WithMessage("Only 250 characters are allowed.");
        }
    }
}
