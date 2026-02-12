using Application.Common.DTO.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Validator
{
    public class UserRequestValidator:AbstractValidator<UserRequest>
    {
        public UserRequestValidator() 
        {
            RuleFor(Request => Request.username)
                .NotEmpty().WithMessage("Username is must have")
                .MaximumLength(250).WithMessage("Only 250 characters are allowed.");
            RuleFor(Request => Request.password)
                .NotEmpty().WithMessage("Password is must have")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(250).WithMessage("Only 250 characters are allowed.");
            RuleFor(Request=>Request.name)
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
