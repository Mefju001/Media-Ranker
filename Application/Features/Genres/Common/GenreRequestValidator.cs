using FluentValidation;

namespace Application.Features.Genres.Common
{
    public class GenreRequestValidator : AbstractValidator<GenreRequest>
    {
        public GenreRequestValidator()
        {
            RuleFor(x => x.name).NotEmpty().WithMessage("Genre name is required.");
        }
    }
}
