using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Shared.Web.Validators
{
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
