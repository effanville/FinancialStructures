using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using System;
using System.Collections.Generic;

namespace FinancialStructures.Database
{
    public static partial class PortfolioSecurity
    {
        /// <summary>
        /// Return alphabetically ordered list of all sectors linked to securities without repetition.
        /// </summary>
        public static List<string> GetSecuritiesSectors(this Portfolio portfolio)
        {
            var companies = new List<string>();
            foreach (var security in portfolio.Funds)
            {
                var sectors = security.GetSectors();
                foreach (var sector in sectors)
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

        public static double SecurityShares(this Portfolio portfolio, string company, string name, DateTime date)
        {
            foreach (Security sec in portfolio.Funds)
            {
                if (sec.GetName() == name && sec.GetCompany() == company)
                {
                    return sec.Shares.NearestEarlierValue(date).Value;
                }
            }

            return 0.0;
        }
    }
}
