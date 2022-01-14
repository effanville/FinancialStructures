using System.Globalization;

namespace FinancialStructures
{
    /// <summary>
    /// Helper methods for obtaining culture info from a currency name.
    /// </summary>
    public static class CurrencyCultureHelpers
    {
        /// <summary>
        /// Returns the standard culture specified from the currency name.
        /// </summary>
        public static string FormatName(string currencyName)
        {
            switch (currencyName)
            {
                case "GBP":
                case "£":
                    return "en-GB";
                case "HKD":
                    return "zh-HK";
                case "USD":
                    return "en-US";
                case "SAR":
                    return "en-ZA";
                default:
                    return currencyName;
            }
        }

        /// <summary>
        /// Returns the standard culture specified from the currency name.
        /// </summary>
        public static CultureInfo CurrencyCultureInfo(string currencyName)
        {
            if (string.IsNullOrEmpty(currencyName))
            {
                return null;
            }

            string cultureSpecifier = FormatName(currencyName);
            CultureInfo cultureInfo;
            try
            {
                cultureInfo = CultureInfo.CreateSpecificCulture(cultureSpecifier);
            }
            catch (CultureNotFoundException)
            {
                return null;
            }

            return cultureInfo;
        }
    }
}
