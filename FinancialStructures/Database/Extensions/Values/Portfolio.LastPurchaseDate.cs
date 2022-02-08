using System;
using System.Collections.Generic;
using System.Linq;
using FinancialStructures.DataStructures;
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
        /// Returns the earliest date held in the portfolio.
        /// </summary>
        /// <param name="portfolio">The database to query</param>
        /// <param name="elementType">The type of element to search for. All searches for Bank accounts and securities.</param>
        /// <param name="name">An ancillary name to use in the case of Sectors</param>
        public static DateTime LastPurchaseDate(this IPortfolio portfolio, Totals elementType, TwoName name = null)
        {
            switch (elementType)
            {
                case Totals.Security:
                {
                    return LastPurchaseDateOf(portfolio.FundsThreadSafe, portfolio);
                }
                case Totals.SecurityCompany:
                {
                    return LastPurchaseDateOf(portfolio.CompanyAccounts(Account.Security, name.Company), portfolio);
                }
                case Totals.Sector:
                case Totals.SecuritySector:
                {
                    return LastPurchaseDateOf(portfolio.SectorAccounts(Account.Security, name), portfolio);
                }
                case Totals.BankAccountCompany:
                case Totals.Company:
                case Totals.BankAccountSector:
                case Totals.CurrencySector:
                case Totals.SecurityCurrency:
                case Totals.BankAccountCurrency:
                case Totals.BankAccount:
                case Totals.Benchmark:
                case Totals.Currency:
                {
                    return default(DateTime);
                }
                case Totals.All:
                default:
                {
                    DateTime earlySecurity = portfolio.LastInvestmentDate(Totals.Security);
                    return earlySecurity;
                }
            }
        }

        private static DateTime LastPurchaseDateOf(IReadOnlyList<IValueList> accounts, IPortfolio portfolio)
        {
            DateTime output = DateTime.MinValue;
            foreach (ISecurity valueList in accounts)
            {
                if (valueList.Any())
                {
                    ICurrency currency = portfolio.Currency(valueList);
                    DateTime latest = valueList.Trades.LastOrDefault(trade => trade.TradeType.Equals(TradeType.Buy))?.Day ?? default(DateTime);
                    if (latest > output)
                    {
                        output = latest;
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
        public static DateTime LastPurchaseDate(this IPortfolio portfolio, Account elementType, TwoName name)
        {
            if (portfolio.TryGetAccount(elementType, name, out IValueList desired))
            {
                if (desired is ISecurity sec)
                {
                    ICurrency currency = portfolio.Currency(sec);
                    DateTime latest = sec.Trades.LastOrDefault(trade => trade.TradeType.Equals(TradeType.Buy))?.Day ?? default(DateTime);
                    return latest;
                }
            }

            return default(DateTime);
        }
    }
}