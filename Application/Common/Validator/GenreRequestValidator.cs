using Application.Common.DTO.Request;
using FluentValidation;

namespace Application.Common.Validator
{
    public class GenreRequestValidator : AbstractValidator<GenreRequest>
    {
        public GenreRequestValidator()
        {
            RuleFor(x => x.name).NotEmpty().WithMessage("Genre name is required.");
        }
    }
}
