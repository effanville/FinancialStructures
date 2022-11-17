using System.Collections.Generic;
using System.Linq;

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
            var values = portfolio.CalculateAggregateStatistic<ISecurity, List<Labelled<TwoName, DailyValuation>>>(
              totals,
              name,
              (tot, n) => tot == Totals.Security
                  || tot == Totals.SecurityCompany
                  || tot == Totals.Sector
                  || tot == Totals.SecuritySector
                  || tot == Totals.All,
              new List<Labelled<TwoName, DailyValuation>>(),
              valueList => Calculate(valueList),
              (date, otherDate) => date.Union(otherDate).ToList());

            values?.Sort();
            return values;
            List<Labelled<TwoName, DailyValuation>> Calculate(ISecurity security)
            {
                ICurrency currency = portfolio.Currency(security);
                return security.AllInvestmentsNamed(currency);
            }
        }

        /// <summary>
        /// Returns the investments in the object specified
        /// </summary>
        /// <param name="portfolio">The database to query.</param>
        /// <param name="account">The type of account to look for.</param>
        /// <param name="name">The name of the account.</param>
        public static List<Labelled<TwoName, DailyValuation>> Investments(this IPortfolio portfolio, Account account, TwoName name)
        {
            return portfolio.CalculateStatistic<ISecurity, List<Labelled<TwoName, DailyValuation>>>(
               account,
               name,
               (acc, n) => acc == Account.Security || acc == Account.Pension,
               security => Calculate(security));
            List<Labelled<TwoName, DailyValuation>> Calculate(ISecurity sec)
            {
                ICurrency currency = portfolio.Currency(sec);
                return sec.AllInvestmentsNamed(currency);
            }
        }
    }
}
