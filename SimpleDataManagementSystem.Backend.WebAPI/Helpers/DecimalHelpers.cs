using System.Globalization;

namespace SimpleDataManagementSystem.Backend.WebAPI.Helpers
{
    public static class DecimalHelpers
    {
        public static decimal GetDecimalFromString(string @decimal)
        {
            var decimalAsString = @decimal.Replace(",", ".");

            Char replaceChar = '.';
            
            int lastIndex = decimalAsString.LastIndexOf(replaceChar);
            
            if (lastIndex != -1)
            {
                IEnumerable<Char> chars = decimalAsString.Where((c, i) => c != replaceChar || i == lastIndex);
                decimalAsString = new string(chars.ToArray());
            }

            decimal d = decimal.Parse(decimalAsString, new NumberFormatInfo() { NumberDecimalSeparator = "." });

            return d;
        }
    }
}
