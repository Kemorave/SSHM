namespace Self_service_hotel_managment.Common.VaildationRule
{
    public class NullValidationRule : System.Windows.Controls.ValidationRule
    {
        
        public override System.Windows.Controls.ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            //System.Windows.Data.Binding
            string result = value as string;
            return string.IsNullOrEmpty(result) ?
                new System.Windows.Controls.ValidationResult(false, "Value can't be empty")
                :
                System.Windows.Controls.ValidationResult.ValidResult;
        }
    }
    public class EmailValidationRule : System.Windows.Controls.ValidationRule
    {
        public override System.Windows.Controls.ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string email = value as string;
            return string.IsNullOrEmpty(email) ?
                new System.Windows.Controls.ValidationResult(false, "Value can't be empty")
                :
                KemoTools.AttachedProperties.TextBoxHelper.VaildEmailRegex.IsMatch(email) ?
                System.Windows.Controls.ValidationResult.ValidResult
                :
                new System.Windows.Controls.ValidationResult(false, "Invail Email");
        }
    }
    public class PasswordValidationRule : System.Windows.Controls.ValidationRule
    {
        public override System.Windows.Controls.ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string email = value as string;
            return string.IsNullOrEmpty(email) ?
                new System.Windows.Controls.ValidationResult(false, "Value can't be empty")
                :
                email?.Length > 8 ?
                System.Windows.Controls.ValidationResult.ValidResult
                :
                new System.Windows.Controls.ValidationResult(false, "Password is too short");
        }
    }
}