using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Core.Rules;
using FluentValidation;

namespace AutoBookKeeper.Application.Validators;

public class BookValidator : AbstractValidator<BookModel>
{
    public BookValidator()
    {
        RuleFor(book => book.Owner)
            .NotEmpty().WithMessage("Owner is required");

        RuleFor(book => book.Title)
            .NotEmpty().WithMessage("Title is required")
            .MinimumLength(BookRules.MinTitleLength)
            .WithMessage($"Title length can't be less than {BookRules.MinTitleLength}.")
            .MaximumLength(BookRules.MaxTitleLength)
            .WithMessage($"Title length can't be more than {BookRules.MaxTitleLength}.");
    }
}