using FinancialStructures.FinanceInterfaces;
using System;
using System.Collections.Generic;

namespace FinancialStructures.Database
{
    public static partial class PortfolioSecurity
    {
        /// <summary>
        /// Return alphabetically ordered list of all sectors linked to securities without repetition.
        /// </summary>
        public static List<string> GetSecuritiesSectors(this IPortfolio portfolio)
        {
            var companies = new List<string>();
            foreach (ISecurity security in portfolio.Funds)
            {
                foreach (var sector in security.Sectors)
                {
                    if (companies.IndexOf(sector) == -1)
                    {
                        companies.Add(sector);
                    }
                }
            }
            companies.Sort();

            return companies;
        }

        public static double SecurityShares(this IPortfolio portfolio, string company, string name, DateTime date)
        {
            foreach (ISecurity sec in portfolio.Funds)
            {
                if (sec.Name == name && sec.Company == company)
                {
                    return sec.Shares.NearestEarlierValue(date).Value;
                }
            }

            return 0.0;
        }
    }
}
