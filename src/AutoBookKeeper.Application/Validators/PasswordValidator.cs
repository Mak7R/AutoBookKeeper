using AutoBookKeeper.Core.Rules;
using FluentValidation;

namespace AutoBookKeeper.Application.Validators;

public record Password(string Value);

public class PasswordValidator : AbstractValidator<Password>
{
    public PasswordValidator()
    {
        RuleFor(p => p.Value)
            .MinimumLength(UserRules.MinPasswordLength).WithMessage($"Password length must be more than {UserRules.MinPasswordLength}")
            .MaximumLength(UserRules.MaxPasswordLength).WithMessage($"Password length must be less than {UserRules.MaxPasswordLength}")
            .Must(ValidatePasswordComplexity)
            .OverridePropertyName(nameof(Password));
    }

    private static bool ValidatePasswordComplexity(string password)
    {
        return true;
    }
}