using System;
using System.Collections.Generic;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;

namespace FinancialStructures.Database.Statistics
{
    public static partial class PortfolioSecurity
    {
        public static DateTime CompanyFirstDate(this IPortfolio portfolio, string company)
        {
            DateTime output = DateTime.Today;
            foreach (ISecurity sec in portfolio.CompanySecurities(company))
            {
                if (sec.FirstValue().Day < output)
                {
                    output = sec.FirstValue().Day;
                }
            }

            return output;
        }

        /// <summary>
        /// Returns a named list of all investments in the company.
        /// </summary>
        public static List<DayValue_Named> CompanyInvestments(this IPortfolio portfolio, string company)
        {
            List<DayValue_Named> output = new List<DayValue_Named>();
            foreach (ISecurity sec in portfolio.CompanySecurities(company))
            {
                ICurrency currency = portfolio.Currency(AccountType.Security, sec);
                output.AddRange(sec.AllInvestmentsNamed(currency));
            }

            return output;
        }
    }
}
