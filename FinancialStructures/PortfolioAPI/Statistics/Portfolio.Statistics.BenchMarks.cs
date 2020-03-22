using FinancialStructures.DatabaseInterfaces;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using System;
using System.Collections.Generic;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioStatistics
    {
        /// <summary>
        /// returns the securities under the company name.
        /// </summary>
        public static SecurityStatsHolder GenerateBenchMarkStatistics(this IPortfolio portfolio, List<Sector> sectors, string sectorName)
        {
            if (portfolio != null)
            {
                var totals = new SecurityStatsHolder(sectorName, "BenchMark");
                portfolio.AddSectorStats(totals, DateTime.Today, sectors);
                return totals;
            }

            return new SecurityStatsHolder();
        }
    }
}
