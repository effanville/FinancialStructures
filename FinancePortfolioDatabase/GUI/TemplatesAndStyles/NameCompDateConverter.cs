using System;
using System.Globalization;
using System.Windows.Data;
using FinancialStructures.NamingStructures;

namespace FinancePortfolioDatabase.GUI
{
    public class NameCompDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is NameCompDate)
            {
                return value;
            }

            return null;
        }
    }
}
