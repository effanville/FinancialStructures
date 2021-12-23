using System;
using System.Collections.Generic;
using System.Linq;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Extensions.Values
{
    /// <summary>
    /// Holds static extension classes generating values data for the portfolio.
    /// </summary>
    public static partial class Values
    {
        /// <summary>
        /// Returns the latest date held in the portfolio.
        /// </summary>
        /// <param name="portfolio">The database to query</param>
        /// <param name="total">The type of element to search for. All searches for Bank accounts and securities.</param>
        /// <param name="name">An ancillary name to use in the case of Sectors</param>
        /// <returns></returns>
        public static DateTime LatestDate(this IPortfolio portfolio, Totals total, TwoName name = null)
        {
            switch (total)
            {
                case Totals.Security:
                {
                    return LatestDateOf(portfolio.FundsThreadSafe);
                }
                case Totals.SecurityCompany:
                case Totals.BankAccountCompany:
                {
                    return LatestDateOf(portfolio.CompanyAccounts(total.ToAccount(), name.Company));
                }
                case Totals.Sector:
                {
                    DateTime earlySecurity = portfolio.LatestDate(Totals.SecuritySector, name);
                    DateTime earlyBank = portfolio.LatestDate(Totals.BankAccountSector, name);

                    if (earlySecurity == default)
                    {
                        return earlyBank;
                    }

                    if (earlyBank == default)
                    {
                        return earlySecurity;
                    }

                    return earlySecurity > earlyBank ? earlySecurity : earlyBank;
                }
                case Totals.SecuritySector:
                case Totals.BankAccountSector:
                {
                    return LatestDateOf(portfolio.SectorAccounts(total.ToAccount(), name));
                }
                case Totals.BankAccount:
                {
                    return LatestDateOf(portfolio.BankAccountsThreadSafe);
                }
                case Totals.Benchmark:
                {
                    return LatestDateOf(portfolio.BenchMarksThreadSafe);
                }
                case Totals.Currency:
                {
                    return LatestDateOf(portfolio.CurrenciesThreadSafe);
                }

                case Totals.Company:
                {
                    DateTime earlySecurity = portfolio.LatestDate(Totals.SecurityCompany, name);
                    DateTime earlyBank = portfolio.LatestDate(Totals.BankAccountCompany, name);

                    if (earlySecurity == default)
                    {
                        return earlyBank;
                    }

                    if (earlyBank == default)
                    {
                        return earlySecurity;
                    }

                    return earlySecurity > earlyBank ? earlySecurity : earlyBank;
                }
                case Totals.CurrencySector:
                case Totals.SecurityCurrency:
                case Totals.BankAccountCurrency:
                {
                    return default;
                }
                case Totals.All:
                default:
                {
                    DateTime earlySecurity = portfolio.LatestDate(Totals.Security);
                    DateTime earlyBank = portfolio.LatestDate(Totals.BankAccount);
                    return earlySecurity > earlyBank ? earlySecurity : earlyBank;
                }
            }
        }

        private static DateTime LatestDateOf(IReadOnlyList<IValueList> accounts)
        {
            if (!accounts.Any())
            {
                return default;
            }

            DateTime output = DateTime.MinValue;
            foreach (IValueList sec in accounts)
            {
                if (sec.Any())
                {
                    DateTime securityEarliest = sec.LatestValue().Day;
                    if (securityEarliest > output)
                    {
                        output = securityEarliest;
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Returns the latest date held in the portfolio.
        /// </summary>
        /// <param name="portfolio">The database to query</param>
        /// <param name="elementType">The type of element to search for. All searches for Bank accounts and securities.</param>
        /// <param name="name">An ancillary name to use in the case of Sectors</param>
        /// <returns></returns>
        public static DateTime LatestDate(this IPortfolio portfolio, Account elementType, TwoName name)
        {
            if (portfolio.TryGetAccount(elementType, name, out IValueList desired))
            {
                return desired.LatestValue()?.Day ?? DateTime.MinValue;
            }

            return default;
        }
    }
}