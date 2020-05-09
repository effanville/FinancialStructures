using System;

namespace UICommon.Converters
{
    public static class DateTimeStringConverter
    {
        /// <summary>
        /// Outputs a date in the UK format (the good format) from a datetime.
        /// </summary>
        public static string ToUKDateString(this DateTime date)
        {
            return date.Day + "/" + date.Month + "/" + date.Year;
        }
    }
}
