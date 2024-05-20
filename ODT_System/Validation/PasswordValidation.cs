using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ODT_System.Validation
{
    public class PasswordValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string password = value as string;

            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            // Check if password has at least 8 characters
            if (password.Length < 8)
            {
                return false;
            }

            // Check if password has at least 1 special character
            if (!Regex.IsMatch(password, @"[!@#$%^&*(),.?\"":{\\|<>]"))
            {
                return false;
            }

            // Check if password has at least 1 number
            if (!Regex.IsMatch(password, @"\d"))
            {
                return false;
            }

            return true;
        }
    }
}
