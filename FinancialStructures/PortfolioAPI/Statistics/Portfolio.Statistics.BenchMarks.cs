using FinancialStructures.FinanceInterfaces;
using FinancialStructures.GUIFinanceStructures;
using System;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioStatistics
    {
        /// <summary>
        /// returns the securities under the company name.
        /// </summary>
        public static SecurityStatsHolder GenerateBenchMarkStatistics(this IPortfolio portfolio, string sectorName)
        {
            if (portfolio != null)
            {
                var totals = new SecurityStatsHolder(sectorName, "BenchMark");
                portfolio.AddSectorStats(totals, DateTime.Today);
                return totals;
            }

            return new SecurityStatsHolder();
        }
    }
}
