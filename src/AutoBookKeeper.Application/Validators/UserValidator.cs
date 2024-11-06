using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Core.Rules;
using FluentValidation;

namespace AutoBookKeeper.Application.Validators;

public class UserValidator : AbstractValidator<UserModel>
{
    public UserValidator()
    {
        RuleFor(u => u.UserName)
            .NotEmpty()
            .MinimumLength(UserRules.MinUserNameLength)
            .MaximumLength(UserRules.MaxUserNameLength);

        RuleFor(u => u.Email)
            .MaximumLength(UserRules.MaxEmailLength)
            .Must(ValidateEmail);
    }

    private static bool ValidateEmail(string email)
    {
        if (string.IsNullOrEmpty(email)) return true;

        var index = email.IndexOf('@');
        if (index < 0)
            return false;

        var emailParts = email.Split('@');
        if (emailParts.Length != 2)
            return false;

        return emailParts[0].Length > 1 && emailParts[1].Length > 1;
    }
}