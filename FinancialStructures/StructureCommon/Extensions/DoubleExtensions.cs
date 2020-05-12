using System;

namespace StructureCommon.Extensions
{
    /// <summary>
    /// Miscellaneous custom extension functions for the <see cref="double"/> type.
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// Truncates the double, and returns the output as a string.
        /// </summary>
        public static string TruncateToString(this double value, int exp = 2)
        {
            double decimalPlaces = Math.Pow(10, exp);
            return (Math.Truncate(value * decimalPlaces) / decimalPlaces).ToString();
        }

        /// <summary>
        /// Truncates the decimals of the double.
        /// </summary>
        public static double Truncate(this double value, int exp = 2)
        {
            double decimalPlaces = Math.Pow(10, exp);
            return (Math.Truncate(value * decimalPlaces) / decimalPlaces);
        }

    }
}
