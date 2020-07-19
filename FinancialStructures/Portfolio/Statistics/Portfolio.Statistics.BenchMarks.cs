using System;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.StatisticStructures;

namespace FinancialStructures.Database.Statistics
{
    public static partial class PortfolioStatisticGenerators
    {
        /// <summary>
        /// returns the securities under the company name.
        /// </summary>
        public static SectorStatistics GenerateBenchMarkStatistics(this IPortfolio portfolio, string sectorName)
        {
            if (portfolio != null)
            {
                SectorStatistics totals = new SectorStatistics(StatisticsType.BenchMarkTotal, new NamingStructures.TwoName("BenchMark", sectorName));
                portfolio.AddSectorStats(totals, DateTime.Today);
                return totals;
            }

            return new SectorStatistics();
        }
    }
}
