using System;

namespace StructureCommon.Extensions
{
    /// <summary>
    /// Miscellaneous custom extension methods for the <see cref="DateTime"/> class.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Returns a string representation of the DateTime in a format suitable to be 
        /// used in filenames.
        /// </summary>
        public static string FileSuitableDateTimeValue(this DateTime dateTime)
        {
            return dateTime.Year.ToString() + dateTime.Month.ToString() + dateTime.Day.ToString() + "-" + dateTime.Hour.ToString() + dateTime.Minute.ToString("D2");
        }

        /// <summary>
        /// Returns a string representation of the DateTime date part only in a 
        /// format suitable to be used in filenames.
        /// </summary>
        public static string FileSuitableUKDateString(this DateTime date)
        {
            return date.Year.ToString() + date.Month.ToString() + date.Day.ToString();
        }
    }
}
