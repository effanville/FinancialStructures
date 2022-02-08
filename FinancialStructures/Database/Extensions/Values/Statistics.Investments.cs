using System.Collections.Generic;
using Common.Structure.DataStructures;
using Common.Structure.NamingStructures;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Extensions.Values
{
    /// <summary>
    /// Class containing static extension methods for values data for a portfolio.
    /// </summary>
    public static partial class Values
    {
        /// <summary>
        /// Returns a list of all the investments in the given type.
        /// </summary>
        /// <param name="portfolio"></param>
        /// <param name="totals"></param>
        /// <param name="name"></param>
        public static List<Labelled<TwoName, DailyValuation>> TotalInvestments(this IPortfolio portfolio, Totals totals, TwoName name = null)
        {
            switch (totals)
            {
                case Totals.Security:
                {
                    return TotalInvestmentsOf(portfolio.FundsThreadSafe, portfolio, totals);
                }
                case Totals.SecurityCompany:
                {
                    return TotalInvestmentsOf(portfolio.CompanyAccounts(Account.Security, name.Company), portfolio, totals);
                }
                case Totals.SecuritySector:
                {
                    return TotalInvestmentsOf(portfolio.SectorAccounts(Account.Security, name), portfolio, totals);
                }
                case Totals.All:
                {
                    return portfolio.TotalInvestments(Totals.Security);
                }
                default:
                {
                    return null;
                }
            }
        }

        private static List<Labelled<TwoName, DailyValuation>> TotalInvestmentsOf(IReadOnlyList<IValueList> securities, IPortfolio portfolio, Totals totals)
        {
            List<Labelled<TwoName, DailyValuation>> output = new List<Labelled<TwoName, DailyValuation>>();
            foreach (ISecurity security in securities)
            {
                ICurrency currency = portfolio.Currency(security);
                output.AddRange(security.AllInvestmentsNamed(currency));
            }

            output.Sort();
            return output;
        }

        /// <summary>
        /// Returns the investments in the object specified
        /// </summary>
        /// <param name="portfolio">The database to query.</param>
        /// <param name="accountType">The type of account to look for.</param>
        /// <param name="names">The name of the account.</param>
        public static List<Labelled<TwoName, DailyValuation>> Investments(this IPortfolio portfolio, Account accountType, TwoName names)
        {
            if (accountType != Account.Security)
            {
                return null;
            }

            if (portfolio.TryGetAccount(Account.Security, names, out IValueList desired) && desired.Any())
            {
                ISecurity security = desired as ISecurity;
                ICurrency currency = portfolio.Currency(security);
                return security.AllInvestmentsNamed(currency);

            }

            return null;
        }
    }
}
