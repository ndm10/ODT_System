using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class EnumValueAttribute : ValidationAttribute
{
    private readonly Type _enumType;

    public EnumValueAttribute(Type enumType)
    {
        _enumType = enumType;
        if (!enumType.IsEnum)
        {
            throw new ArgumentException("Type must be an enum", nameof(enumType));
        }
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        // Check for null, empty, or whitespace strings
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            // If you want to allow empty or whitespace strings, return ValidationResult.Success;
            // Otherwise, return an error message indicating that a value is required.
            return new ValidationResult($"The {validationContext.DisplayName} field requires a non-empty value.");
        }

        string valueAsString = value.ToString();
        bool isValid = Enum.GetNames(_enumType).Any(name => string.Equals(name, valueAsString, StringComparison.OrdinalIgnoreCase));

        if (isValid)
        {
            return ValidationResult.Success;
        }
        else
        {
            var validValues = string.Join(", ", Enum.GetNames(_enumType));
            return new ValidationResult($"The value '{valueAsString}' is not valid for {validationContext.DisplayName}. Please enter one of the following values: {validValues}.");
        }
    }
}
