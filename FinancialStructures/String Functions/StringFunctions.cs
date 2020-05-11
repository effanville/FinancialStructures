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

        /// <summary>
        /// Convert a string into the enum type given by T.
        /// </summary>
        public static T ToEnum<T>(this string value)
        {
            T output = default(T);
            output = (T)Enum.Parse(typeof(T), value, true);
            return output;
        }

        /// <summary>
        /// Returns a string representation of the DateTime in a format suitable to be 
        /// used in filenames.
        /// </summary>
        public static string FileSuitableDateTimeValue(this DateTime dateTime)
        {
            return dateTime.Year.ToString() + dateTime.Month.ToString() + dateTime.Day.ToString() + "-" + dateTime.Hour.ToString() + dateTime.Minute.ToString("D2");
        }

        public static string FileSuitableUKDateString(this DateTime date)
        {
            return date.Year.ToString() + date.Month.ToString() + date.Day.ToString();
        }
    }
}
