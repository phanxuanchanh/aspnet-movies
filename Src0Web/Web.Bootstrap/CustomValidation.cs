using System;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace Web
{
    public class CustomValidation
    {
        public void Init(CustomValidator customValidator, string controlToValidate, string errorMessage, bool validateEmptyText, string clientValidationFunction, ServerValidateEventHandler serverValidateEventHandler)
        {
            customValidator.ErrorMessage = errorMessage;
            customValidator.ValidateEmptyText = validateEmptyText;
            customValidator.ControlToValidate = controlToValidate;
            customValidator.ServerValidate += serverValidateEventHandler;
            customValidator.ClientValidationFunction = clientValidationFunction;
        }

        public void ValidateEmail(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(
                args.Value,
                @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                    @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
            );
        }

        public void ValidatePassword(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, "^[0-9a-zA-Z$%#@!*+-,.?/]{6,30}$");
        }

        public void ValidateUsername(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, "^[a-z0-9_-]{3,15}$");
        }

        public void ValidatePhoneNumber(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, "^[0-9]{3,15}$");
        }

        public void ValidateConfirmCode(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, "^[0-9]{6,12}$");
        }

        public void ValidateCardNumber(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, "^[0-9]{16,19}$");
        }

        public void ValidateCVV(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, "^[0-9]{3}$");
        }

        public void ValidateAccountName(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, "^[A-Za-z0-9 ]{2,}$");
        }

        public void ValidateExpirationDate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, @"^(0[1-9]|1[0-2])\/?(([0-9]{4}|[0-9]{2})$)");
        }

        public void ValidateCategoryName(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, @"^[0-9\w-_. ]{3,50}$");
        }

        public void ValidateRoleName(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, @"^[0-9\w-_. ]{3,50}$");
        }

        public void ValidateCountryName(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, @"^[0-9\w-_. ]{3,50}$");
        }

        public void ValidateLanguageName(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, @"^[0-9\w-_. ]{3,50}$");
        }

        public void ValidateDirectorName(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, @"^[0-9\w-_. ]{3,50}$");
        }

        public void ValidateCastName(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, @"^[0-9\w-_. ]{3,50}$");
        }

        public void ValidateTagName(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, @"^[0-9\w-_. ]{3,50}$");
        }

        public void ValidateFilmName(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, @"^[0-9\w-_. ]{3,50}$");
        }

        public void ValidateProductionCompany(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, @"^[0-9\w-_. ]{3,50}$");
        }

        public void ValidateReleaseDate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, @"^[0-9]{4,10}$");
        }

        public void ValidateDirectorRole(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, @"^[0-9\w-_. ]{3,50}$");
        }

        public void ValidateCastRole(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, @"^[0-9\w-_. ]{3,50}$");
        }
    }
}