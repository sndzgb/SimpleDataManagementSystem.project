using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Shared.Extensions
{
    public static class StringExtensions
    {
        public static string SplitCamelCase(this string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(
                input, "(?<=[a-z])([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled
            ).Trim();
        }
    }
}
