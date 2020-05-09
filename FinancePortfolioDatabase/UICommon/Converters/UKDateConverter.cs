using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace UICommon.Converters
{
    public class StringToUKDateConverter : MarkupExtension, IValueConverter
    {
        private static StringToUKDateConverter sConverter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (sConverter == null)
            {
                sConverter = new StringToUKDateConverter();
            }

            return sConverter;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                return date.ToUKDateString();
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                if (culture.IetfLanguageTag == "en-US")
                {
                    CultureInfo UKEnglishCulture = new CultureInfo("en-GB");
                    if (DateTime.TryParse(stringValue, UKEnglishCulture.DateTimeFormat, DateTimeStyles.None, out DateTime date))
                    {
                        return date;
                    }
                }
                else
                {
                    if (DateTime.TryParse(stringValue, culture.DateTimeFormat, DateTimeStyles.None, out DateTime date))
                    {
                        return date;
                    }
                }
            }
            return null;
        }
    }
}
