using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.ReportingStructures;
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

        public static bool TryAddCurrency(this Portfolio portfolio, NameData name, ErrorReports reports)
        {
            return portfolio.TryAddCurrency(name.Name, name.Url, reports);
        }

        /// <summary>
        /// Tries to add a sector to the underlying global database
        /// </summary>
        public static bool TryAddCurrency(this Portfolio portfolio, string name, string url, ErrorReports reports)
        {
            foreach (var sector in portfolio.Currencies)
            {
                if (name == sector.GetName())
                {
                    reports.AddError($"Sector with name {name} already exists.", Location.AddingData);
                    return false;
                }
            }
            Currency newSector = new Currency(name, url);
            reports.AddReport($"Added sector with name {name}.", Location.AddingData);
            portfolio.Currencies.Add(newSector);
            return true;
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

        public static List<AccountDayDataView> CurrencyData(this Portfolio portfolio, NameData name, ErrorReports reports)
        {
            foreach (Currency sec in portfolio.Currencies)
            {
                if (sec.GetName() == name.Name)
                {
                    return sec.GetDataForDisplay();
                }
            }

            reports.AddError($"Could not find currency {name.Name}", Location.DatabaseAccess);
            return new List<AccountDayDataView>();
        }

        public static bool TryGetCurrencyData(this Portfolio portfolio, string name, out List<AccountDayDataView> data)
        {
            data = new List<AccountDayDataView>();
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

        public static bool TryAddDataToCurrency(this Portfolio portfolio, NameData name, AccountDayDataView value, ErrorReports reports)
        {
            return portfolio.TryAddDataToCurrency(name.Name, value.Date, value.Amount, reports);
        }

        /// <summary>
        /// Attempts to add data to the sector. Fails if data already exists
        /// </summary>
        public static bool TryAddDataToCurrency(this Portfolio portfolio, string name, DateTime date, double value, ErrorReports reports)
        {
            foreach (var sector in portfolio.Currencies)
            {
                if (name == sector.GetName())
                {
                    return sector.TryAddData(date, value, reports);
                }
            }

            return false;
        }

        public static bool TryEditCurrency(this Portfolio portfolio, NameData name, AccountDayDataView oldDate, AccountDayDataView value, ErrorReports reports)
        {
            return portfolio.TryEditCurrency(name.Name, oldDate.Date, value.Date, value.Amount, reports);
        }

        public static bool TryEditCurrency(this Portfolio portfolio, string name, DateTime oldDate, DateTime date, double value, ErrorReports reports)
        {
            foreach (var sector in portfolio.Currencies)
            {
                if (name == sector.GetName())
                {
                    return sector.TryEditData(oldDate, date, value, reports);
                }
            }

            return false;
        }

        public static bool TryDeleteCurrencyData(this Portfolio portfolio, NameData name, AccountDayDataView value, ErrorReports reports)
        {
            return portfolio.TryDeleteCurrencyData(name.Name, value.Date, value.Amount, reports);
        }

        public static bool TryDeleteCurrencyData(this Portfolio portfolio, string name, DateTime date, double value, ErrorReports reports)
        {
            foreach (var sector in portfolio.Currencies)
            {
                if (name == sector.GetName())
                {
                    return sector.TryDeleteData(date, value, reports);
                }
            }

            return false;
        }

        public static bool TryEditCurrencyName(this Portfolio portfolio, NameData oldName, NameData newName, ErrorReports reports)
        {
            return portfolio.TryEditCurrencyName(oldName.Name, newName.Name, newName.Url, reports);
        }

        public static bool TryEditCurrencyName(this Portfolio portfolio, string oldName, string newName, string url, ErrorReports reports)
        {
            foreach (var sector in portfolio.Currencies)
            {
                if (sector.GetName() == oldName)
                {
                    sector.TryEditNameUrl(newName, url);
                    reports.AddReport($"Renamed sector {oldName} with new name {newName}.", Location.EditingData);
                    return true;
                }
            }

            reports.AddError($"Could not rename sector {oldName} with new name {newName}.", Location.EditingData);
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

        public static bool TryDeleteCurrency(this Portfolio portfolio, string name)
        {
            foreach (var sector in portfolio.Currencies)
            {
                if (name == sector.GetName())
                {
                    portfolio.Currencies.Remove(sector);
                    return true;
                }
            }

            return false;
        }

        public static bool TryDeleteCurrency(this Portfolio portfolio, NameData name, ErrorReports reports)
        {
            return portfolio.TryDeleteCurrency(name.Name, reports);
        }

        /// <summary>
        /// Deletes sector if sector exists. Does nothing otherwise.
        /// </summary>
        public static bool TryDeleteCurrency(this Portfolio portfolio, string name, ErrorReports reports)
        {
            foreach (var sector in portfolio.Currencies)
            {
                if (name == sector.GetName())
                {
                    reports.AddReport($"Deleted sector {sector.GetName()}", Location.DeletingData);
                    portfolio.Currencies.Remove(sector);
                    return true;
                }
            }

            return false;
        }
    }
}
