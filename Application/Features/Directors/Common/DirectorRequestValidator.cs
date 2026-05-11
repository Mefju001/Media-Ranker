using FluentValidation;

namespace Application.Features.Directors.Common
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
