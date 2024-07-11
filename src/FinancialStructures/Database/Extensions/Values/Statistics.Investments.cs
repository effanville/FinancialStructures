using System.Collections.Generic;
using System.Linq;

using Effanville.Common.Structure.DataStructures;
using Effanville.Common.Structure.NamingStructures;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Extensions.Values
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
            var values = portfolio.CalculateAggregateValue(
              totals,
              name,
              (tot, _) => tot == Totals.Security
                  || tot == Totals.SecurityCompany
                  || tot == Totals.Sector
                  || tot == Totals.SecuritySector
                  || tot == Totals.All,
              new List<Labelled<TwoName, DailyValuation>>(),
              (date, otherDate) => date.Union(otherDate).ToList(),
              s => Calculate(portfolio, s),
              defaultValue: new List<Labelled<TwoName, DailyValuation>>());

            values?.Sort();
            return values;
        }

        /// <summary>
        /// Returns the investments in the object specified
        /// </summary>
        /// <param name="portfolio">The database to query.</param>
        /// <param name="account">The type of account to look for.</param>
        /// <param name="name">The name of the account.</param>
        public static List<Labelled<TwoName, DailyValuation>> Investments(this IPortfolio portfolio, Account account, TwoName name)
        {
            return portfolio.CalculateValue(
                account,
                name,
                s => Calculate(portfolio, s));
        }            
        
        static List<Labelled<TwoName, DailyValuation>> Calculate(IPortfolio portfolio, IValueList valueList)
        {
            if (valueList is not ISecurity sec)
            {
                return new List<Labelled<TwoName, DailyValuation>>(); 
            }

            ICurrency currency = portfolio.Currency(sec);
            return sec.AllInvestmentsNamed(currency);
        }
    }
}
