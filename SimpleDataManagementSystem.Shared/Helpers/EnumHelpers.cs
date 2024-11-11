using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Shared.Helpers
{
    public static class EnumHelpers
    {
        public static bool IsDefined<T>(int value)
        {
            var values = Enum.GetValues(typeof(T)).Cast<int>().OrderBy(x => x);

            return values.Contains(value);
        }
    }
}
