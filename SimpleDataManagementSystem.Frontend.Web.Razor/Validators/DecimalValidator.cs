using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Validators
{
    // TODO move to shared project
    public class DecimalValidator : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            decimal nr;

            bool isDecimal = decimal.TryParse(value.ToString(), CultureInfo.InvariantCulture, out nr);

            if (!isDecimal)
            {
                ErrorMessage = "Invalid decimal value";
            }

            return isDecimal;
        }
    }
}
