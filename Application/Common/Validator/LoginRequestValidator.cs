using FluentValidation;
using Application.Common.DTO.Request;

namespace Application.Common.Validator
{
    public class LoginRequestValidator:AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator() 
        {
            RuleFor(x => x.username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
                .MaximumLength(250).WithMessage("Only 250 characters are allowed.");
            RuleFor(x => x.password)
                .NotEmpty().WithMessage("Password is must have")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(250).WithMessage("Only 250 characters are allowed.");
        }
    }
}
