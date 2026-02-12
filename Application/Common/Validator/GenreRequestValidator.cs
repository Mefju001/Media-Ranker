using FluentValidation;
using Application.Common.DTO.Request;

namespace Application.Common.Validator
{
    public class GenreRequestValidator:AbstractValidator<GenreRequest>
    {
        public GenreRequestValidator() 
        {
            RuleFor(x => x.name).NotEmpty().WithMessage("Genre name is required.");
        }
    }
}
