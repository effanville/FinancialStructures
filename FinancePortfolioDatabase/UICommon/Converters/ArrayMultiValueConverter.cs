using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace UICommon.Converters
{
    /// <summary>
    /// Converter to convert an array of objects into a single object (and not back again)
    /// </summary>
    public class ArrayMultiValueConverter : MarkupExtension, IMultiValueConverter
    {
        private static ArrayMultiValueConverter sConverter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (sConverter == null)
            {
                sConverter = new ArrayMultiValueConverter();
            }

            return sConverter;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
