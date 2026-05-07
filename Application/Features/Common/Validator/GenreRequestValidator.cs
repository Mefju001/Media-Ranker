using Application.Features.Common.DTO.Request;
using FluentValidation;

namespace Application.Features.Common.Validator
{
    public class GenreRequestValidator : AbstractValidator<GenreRequest>
    {
        public GenreRequestValidator()
        {
            RuleFor(x => x.name).NotEmpty().WithMessage("Genre name is required.");
        }
    }
}
