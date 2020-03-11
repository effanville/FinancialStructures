using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using System;
using System.Collections.Generic;

namespace FinancialStructures.Database
{
    public static class PortfolioCurrency
    {
        public static double CurrencyValue(this Portfolio portfolio, string currencyName, DateTime date)
        {
            foreach (var currency in portfolio.Currencies)
            {
                if (currency.GetName() == currencyName)
                {
                    return currency.Value(date).Value;
                }
            }

            return 1.0;
        }

        /// <summary>
        /// Returns a sector from the database with specified name.
        /// </summary>
        public static bool TryGetCurrency(this Portfolio portfolio, string name, out Currency Desired)
        {
            foreach (var sector in portfolio.Currencies)
            {
                if (name == sector.GetName())
                {
                    Desired = sector.Copy();
                }
            }
            Desired = null;
            return false;
        }

        public static List<DayValue_ChangeLogged> CurrencyData(this Portfolio portfolio, NameData name, Action<string, string, string> reportLogger)
        {
            foreach (Currency sec in portfolio.Currencies)
            {
                if (sec.GetName() == name.Name)
                {
                    return sec.GetDataForDisplay();
                }
            }

            reportLogger("Error", "DatabaseAccess", $"Could not find currency {name.Name}");
            return new List<DayValue_ChangeLogged>();
        }

        public static bool TryGetCurrencyData(this Portfolio portfolio, string name, out List<DayValue_ChangeLogged> data)
        {
            data = new List<DayValue_ChangeLogged>();
            foreach (Currency sec in portfolio.Currencies)
            {
                if (sec.GetName() == name)
                {
                    data = sec.GetDataForDisplay();
                    return true;
                }
            }

            return false;
        }

        public static bool TryAddDataToCurrency(this Portfolio portfolio, NameData name, DayValue_ChangeLogged value, Action<string, string, string> reportLogger)
        {
            return portfolio.TryAddDataToCurrency(name.Name, value.Day, value.Value, reportLogger);
        }

        /// <summary>
        /// Attempts to add data to the sector. Fails if data already exists
        /// </summary>
        public static bool TryAddDataToCurrency(this Portfolio portfolio, string name, DateTime date, double value, Action<string, string, string> reportLogger)
        {
            foreach (var sector in portfolio.Currencies)
            {
                if (name == sector.GetName())
                {
                    return sector.TryAddData(date, value, reportLogger);
                }
            }

            return false;
        }

        public static bool TryEditCurrency(this Portfolio portfolio, NameData name, DayValue_ChangeLogged oldDate, DayValue_ChangeLogged value, Action<string, string, string> reportLogger)
        {
            return portfolio.TryEditCurrency(name.Name, oldDate.Day, value.Day, value.Value, reportLogger);
        }

        public static bool TryEditCurrency(this Portfolio portfolio, string name, DateTime oldDate, DateTime date, double value, Action<string, string, string> reportLogger)
        {
            foreach (var sector in portfolio.Currencies)
            {
                if (name == sector.GetName())
                {
                    return sector.TryEditData(oldDate, date, value, reportLogger);
                }
            }

            return false;
        }

        public static bool TryDeleteCurrencyData(this Portfolio portfolio, NameData name, DayValue_ChangeLogged value, Action<string, string, string> reportLogger)
        {
            return portfolio.TryDeleteCurrencyData(name.Name, value.Day, value.Value, reportLogger);
        }

        public static bool TryDeleteCurrencyData(this Portfolio portfolio, string name, DateTime date, double value, Action<string, string, string> reportLogger)
        {
            foreach (var currency in portfolio.Currencies)
            {
                if (name == currency.GetName())
                {
                    return currency.TryDeleteData(date, reportLogger);
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if sector with given name exists.
        /// </summary>
        public static bool DoesCurrencyExist(this Portfolio portfolio, string name)
        {
            foreach (var sector in portfolio.Currencies)
            {
                if (name == sector.GetName())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
