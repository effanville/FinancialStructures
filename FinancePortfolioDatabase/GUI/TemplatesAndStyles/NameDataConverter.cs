using System;
using System.Globalization;
using System.Windows.Data;
using FinancialStructures.NamingStructures;

namespace FinancePortfolioDatabase.GUI
{
    /// <summary>
    /// Converts to and from an object a <see cref="NameData"/> instance.
    /// Used in <see cref="Windows.DataNamesView"/> to enable
    /// adding of objects to the datagrid.
    /// </summary>
    public class NameDataConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is NameData)
            {
                return value;
            }

            return null;
        }
    }
}
