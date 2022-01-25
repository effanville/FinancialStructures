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
        /// Returns the earliest date held in the portfolio.
        /// </summary>
        /// <param name="portfolio">The database to query</param>
        /// <param name="total">The type of element to search for. All searches for Bank accounts and securities.</param>
        /// <param name="name">An ancillary name to use in the case of Sectors</param>
        public static DateTime FirstValueDate(this IPortfolio portfolio, Totals total, TwoName name = null)
        {
            switch (total)
            {
                case Totals.Security:
                {
                    return FirstValueOf(portfolio.FundsThreadSafe);
                }
                case Totals.AssetCompany:
                case Totals.SecurityCompany:
                case Totals.BankAccountCompany:
                {
                    return FirstValueOf(portfolio.CompanyAccounts(total.ToAccount(), name.Company));
                }
                case Totals.Asset:
                {
                    return FirstValueOf(portfolio.Assets);
                }
                case Totals.Sector:
                {
                    return FirstValueOf(portfolio.SectorAccounts(Account.All, name));
                }
                case Totals.SecuritySector:
                case Totals.BankAccountSector:
                case Totals.AssetSector:
                {
                    return FirstValueOf(portfolio.SectorAccounts(total.ToAccount(), name));
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
                case Totals.Company:
                {
                    DateTime earlySecurity = portfolio.FirstValueDate(Totals.SecurityCompany, name);
                    DateTime earlyBank = portfolio.FirstValueDate(Totals.BankAccountCompany, name);

                    if (earlySecurity == default(DateTime))
                    {
                        return earlyBank;
                    }

                    if (earlyBank == default(DateTime))
                    {
                        return earlySecurity;
                    }

                    return earlySecurity < earlyBank ? earlySecurity : earlyBank;
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
                    DateTime earlySecurity = portfolio.FirstValueDate(Totals.Security);
                    DateTime earlyBank = portfolio.FirstValueDate(Totals.BankAccount);
                    return earlySecurity < earlyBank ? earlySecurity : earlyBank;
                }
            }
        }

        private static DateTime FirstValueOf(IReadOnlyList<IValueList> accounts)
        {
            if (!accounts.Any())
            {
                return default;
            }

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

            return default;
        }
    }
}
