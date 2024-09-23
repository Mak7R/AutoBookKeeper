using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AutoBookKeeper.Web.Validators;

public class AllowedCharactersAttribute : ValidationAttribute
{
    private readonly string _allowedCharacters;

    public AllowedCharactersAttribute(string allowedCharacters)
    {
        _allowedCharacters = allowedCharacters;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success;

        if (value is not string stringValue)
            return new ValidationResult("The value is not a string");

        if (stringValue.Any(charValue => !_allowedCharacters.Contains(charValue)))
        {
            return new ValidationResult(
                $"The field can only contain the following characters: {_allowedCharacters}");
        }
        
        return ValidationResult.Success;
    }
}