using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Core.Rules;
using FluentValidation;

namespace AutoBookKeeper.Application.Validators;

public class TransactionValidator : AbstractValidator<TransactionModel>
{
    public TransactionValidator()
    {
        RuleFor(t => t.NameIdentifier)
            .MinimumLength(TransactionRules.MinNameIdentifierLength)
            .MaximumLength(TransactionRules.MaxNameIdentifierLength);

        RuleFor(t => t.Description)
            .MaximumLength(TransactionRules.MaxDescriptionLength);
    }
}