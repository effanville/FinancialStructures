using System;

namespace StringFunctions
{
    /// <summary>
    /// Static extension methods for strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// If the length of the string is over maxLength, then this returns the truncated string.
        /// </summary>
        public static string WithMaxLength(this string value, int maxLength)
        {
            if (value == null)
            {
                return null;
            }

            return value.Substring(0, Math.Min(value.Length, maxLength));
        }
    }
}
