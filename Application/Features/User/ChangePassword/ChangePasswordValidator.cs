using FluentValidation;

namespace Application.Features.User.ChangePassword
{
    internal class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.oldPassword)
                .NotEmpty().WithMessage("Stare hasło jest wymagane.");

            RuleFor(x => x.newPassword)
                .NotEmpty().WithMessage("Nowe hasło jest wymagane.")
                .MinimumLength(8).WithMessage("Hasło musi mieć co najmniej 8 znaków.")
                .MaximumLength(100).WithMessage("Hasło jest zbyt długie.")
                .Matches(@"[A-Z]").WithMessage("Hasło musi zawierać co najmniej jedną wielką literę.")
                .Matches(@"[a-z]").WithMessage("Hasło musi zawierać co najmniej jedną małą literę.")
                .Matches(@"[0-9]").WithMessage("Hasło musi zawierać co najmniej jedną cyfrę.")
                .Matches(@"[\!\?\*\.]").WithMessage("Hasło musi zawierać znak specjalny (!?*.).")
                .NotEqual(x => x.oldPassword).WithMessage("Nowe hasło nie może być takie samo jak stare.");

            RuleFor(x => x.confirmPassword)
                .Equal(x => x.newPassword).WithMessage("Hasła nie są identyczne.");
        }
    }
}
