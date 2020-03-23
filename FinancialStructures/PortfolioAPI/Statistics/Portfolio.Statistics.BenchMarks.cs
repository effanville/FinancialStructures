using FinancialStructures.FinanceInterfaces;
using FinancialStructures.StatisticStructures;
using System;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioStatistics
    {
        /// <summary>
        /// returns the securities under the company name.
        /// </summary>
        public static SecurityStatistics GenerateBenchMarkStatistics(this IPortfolio portfolio, string sectorName)
        {
            if (portfolio != null)
            {
                var totals = new SecurityStatistics(StatisticsType.BenchMarkTotal, new NamingStructures.TwoName("BenchMark", sectorName));
                portfolio.AddSectorStats(totals, DateTime.Today);
                return totals;
            }

            return new SecurityStatistics();
        }
    }
}
