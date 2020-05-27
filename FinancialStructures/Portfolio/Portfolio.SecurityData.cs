using System;
using System.Collections.Generic;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database
{
    public enum SecurityDataStream
    {
        NumberOfShares,
        SharePrice
    }

    public static partial class PortfolioSecurity
    {
        /// <summary>
        /// Return alphabetically ordered list of all sectors linked to securities without repetition.
        /// </summary>
        public static List<string> GetSecuritiesSectors(this IPortfolio portfolio)
        {
            List<string> companies = new List<string>();
            foreach (ISecurity security in portfolio.Funds)
            {
                foreach (string sector in security.Sectors)
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

        public static double SecurityPrices(this IPortfolio portfolio, TwoName names, DateTime date, SecurityDataStream dataStream = SecurityDataStream.NumberOfShares)
        {
            foreach (ISecurity sec in portfolio.Funds)
            {
                if (sec.Names.IsEqualTo(names))
                {
                    if (dataStream.Equals(SecurityDataStream.NumberOfShares))
                    {
                        return sec.Shares.NearestEarlierValue(date).Value;
                    }
                    else
                    {
                        return sec.UnitPrice.NearestEarlierValue(date).Value;
                    }
                }
            }

            return 0.0;
        }
    }
}
