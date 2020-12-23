using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialStructures.StatisticStructures;

namespace FinancialStructures.Database.Statistics
{
    /// <summary>
    /// Holds static extension classes generating statistics for the portfolio.
    /// </summary>
    public static partial class PortfolioStatisticGenerators
    {
        public static async Task<List<PortfolioDaySnapshot>> GenerateHistoryStats(this IPortfolio portfolio, int daysGap)
        {
            List<PortfolioDaySnapshot> outputs = new List<PortfolioDaySnapshot>();
            if (!daysGap.Equals(0))
            {
                DateTime calculationDate = portfolio.FirstValueDate(Totals.Security);
                await Task.Run(() => BackGroundTask(calculationDate, portfolio, outputs, daysGap));
            }
            return outputs;
        }

        private static void BackGroundTask(DateTime calculationDate, IPortfolio portfolio, List<PortfolioDaySnapshot> outputs, int daysGap)
        {
            while (calculationDate < DateTime.Today)
            {
                PortfolioDaySnapshot calcuationDateStatistics = new PortfolioDaySnapshot(calculationDate, portfolio);
                outputs.Add(calcuationDateStatistics);
                calculationDate = calculationDate.AddDays(daysGap);
            }
            if (calculationDate == DateTime.Today)
            {
                PortfolioDaySnapshot calcuationDateStatistics = new PortfolioDaySnapshot(calculationDate, portfolio);
                outputs.Add(calcuationDateStatistics);
            }
        }
    }
}
