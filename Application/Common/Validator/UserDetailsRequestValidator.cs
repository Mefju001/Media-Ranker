using Application.Common.DTO.Request;
using FluentValidation;

namespace Application.Common.Validator
{
    public class UserDetailsRequestValidator : AbstractValidator<UserDetailsRequest>
    {
        public UserDetailsRequestValidator()
        {
            RuleFor(Request => Request.name)
                .NotEmpty().WithMessage("Name is must have")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters long.")
                .MaximumLength(250).WithMessage("Only 250 characters are allowed.");
            RuleFor(Request => Request.surname)
                .NotEmpty().WithMessage("Surname is must have")
                .MinimumLength(2).WithMessage("Surname must be at least 2 characters long.")
                .MaximumLength(250).WithMessage("Only 250 characters are allowed.");
            RuleFor(Request => Request.email)
                .NotEmpty().WithMessage("Email is must have")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
