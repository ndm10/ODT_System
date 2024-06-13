using System.ComponentModel.DataAnnotations;

namespace ODT_System.Validation
{
    public class TimeBeforeAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public TimeBeforeAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var currentValue = (TimeOnly)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
                throw new ArgumentException($"Property with name {_comparisonProperty} not found");

            var comparisonValue = (TimeOnly)property.GetValue(validationContext.ObjectInstance);

            if (currentValue < comparisonValue)
                return ValidationResult.Success;
            else
                return new ValidationResult(ErrorMessage);
        }
    }

}
