using System;
using System.Linq;

namespace FinancialStructures.ExpensesStructures.Helpers
{
    internal static class EnumHelper
    {
        public static int LongestEntry<T>() where T : Enum
        {
            string[] values = Enum.GetValues(typeof(T)).Cast<T>().Select(item => item.ToString()).ToArray();
            return values.Max(item => item.Length);
        }
    }
}
