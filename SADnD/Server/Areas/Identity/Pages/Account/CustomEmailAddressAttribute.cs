using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SADnD.Server.Areas.Identity.Pages.Account {
    public class CustomEmailAddressAttribute : ValidationAttribute {
        private const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            var email = value as string;

            if (string.IsNullOrEmpty(email)) {
                return new ValidationResult("Die E-Mail-Adresse ist erforderlich.");
            }

            if (!Regex.IsMatch(email, EmailPattern)) {
                return new ValidationResult("Die E-Mail-Adresse muss eine gültige Domain enthalten (z. B. test@domain.com).");
            }

            return ValidationResult.Success;
        }
    }
}
