using Application.Features.Common.DTO.Request;
using FluentValidation;

namespace Application.Features.Common.Validator
{
    public class DirectorRequestValidator : AbstractValidator<DirectorRequest>
    {
        public DirectorRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Director name is required.");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Director surname is required.");
        }

    }
}
