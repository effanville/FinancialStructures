using System;
using System.Globalization;

namespace FinancialStructures
{
    /// <summary>
    /// Helper methods for obtaining culture info from a currency name.
    /// </summary>
    public static class CurrencyCultureHelpers
    {
        /// <summary>
        /// Provides standard conversion of a value into a value based upon a currency name.
        /// </summary>
        /// <typeparam name="T">Any type that implements <see cref="IFormattable"/></typeparam>
        /// <param name="valueToConvert">The value to output in the currency.</param>
        /// <param name="currencyName">The name of the currency.</param>
        /// <returns>A string with the currency symbol, or the name prefixed if not found.</returns>
        public static string WithCurrencySymbol<T>(this T valueToConvert, string currencyName) where T : IFormattable
        {
            CultureInfo culture = CurrencyCultureInfo(currencyName);
            if (culture == null || string.IsNullOrEmpty(culture.Name))
            {
                return $"{currencyName}{valueToConvert}";
            }

            return valueToConvert.ToString("C", culture);
        }

        /// <summary>
        /// Returns the standard culture specified from the currency name.
        /// </summary>
        private static CultureInfo CurrencyCultureInfo(string currencyName)
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

        /// <summary>
        /// Returns the standard culture specified from the currency name.
        /// </summary>
        private static string FormatName(string currencyName)
        {
            switch (currencyName)
            {
                case "GBP":
                case "£":
                    return "en-GB";
                case "HKD":
                    return "zh-HK";
                case "EUR":
                case "€":
                    return "en-FR";
                case "USD":
                case "$":
                    return "en-US";
                case "SAR":
                    return "en-ZA";
                default:
                    return currencyName;
            }
        }
    }
}
