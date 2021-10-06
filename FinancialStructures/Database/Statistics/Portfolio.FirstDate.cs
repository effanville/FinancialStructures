using System;
using System.Collections.Generic;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics
{
    /// <summary>
    /// Holds static extension classes generating statistics for the portfolio.
    /// </summary>
    public static partial class PortfolioStatisticGenerators
    {
        /// <summary>
        /// Returns the earliest date held in the portfolio.
        /// </summary>
        /// <param name="portfolio">The database to query</param>
        /// <param name="elementType">The type of element to search for. All searches for Bank accounts and securities.</param>
        /// <param name="name">An ancillary name to use in the case of Sectors</param>
        public static DateTime FirstValueDate(this IPortfolio portfolio, Totals elementType, TwoName name = null)
        {
            switch (elementType)
            {
                case Totals.Security:
                {
                    return FirstValueOf(portfolio.FundsThreadSafe);
                }
                case Totals.SecurityCompany:
                {
                    return FirstValueOf(portfolio.CompanyAccounts(Account.Security, name.Company));
                }
                case Totals.Sector:
                case Totals.SecuritySector:
                {
                    return FirstValueOf(portfolio.SectorAccounts(Account.Security, name));
                }
                case Totals.BankAccount:
                {
                    return FirstValueOf(portfolio.BankAccountsThreadSafe);
                }
                case Totals.Benchmark:
                {
                    return FirstValueOf(portfolio.BenchMarksThreadSafe);
                }
                case Totals.Currency:
                {
                    return FirstValueOf(portfolio.CurrenciesThreadSafe);
                }
                case Totals.All:
                default:
                {
                    DateTime earlySecurity = portfolio.FirstValueDate(Totals.Security);
                    DateTime earlyBank = portfolio.FirstValueDate(Totals.BankAccount);
                    return earlySecurity < earlyBank ? earlySecurity : earlyBank;
                }
            }
        }

        private static DateTime FirstValueOf(IReadOnlyList<IValueList> accounts)
        {
            DateTime output = DateTime.MaxValue;
            foreach (IValueList valueList in accounts)
            {
                if (valueList.Any())
                {
                    DateTime earliest = valueList.FirstValue().Day;
                    if (earliest < output)
                    {
                        output = earliest;
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
        public static DateTime FirstDate(this IPortfolio portfolio, Account elementType, TwoName name)
        {
            if (portfolio.TryGetAccount(elementType, name, out IValueList desired))
            {
                return desired.FirstValue()?.Day ?? DateTime.MaxValue;
            }

            return DateTime.MaxValue;
        }
    }
}