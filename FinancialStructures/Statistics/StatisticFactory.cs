using FinancialStructures.Database;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Statistics
{
    /// <summary>
    /// Provides mechanisms to generate statistics objects.
    /// </summary>
    public static class StatisticFactory
    {
        /// <summary>
        /// Generates a statistic class from the specific type enum.
        /// </summary>
        /// <param name="statTypeToGenerate">The <see cref="Statistic"/> to generate.</param>
        /// <returns>A statistic with the relevant type and no value set.</returns>
        public static IStatistic Generate(Statistic statTypeToGenerate)
        {
            switch (statTypeToGenerate)
            {
                case Statistic.Company:
                    return new StatisticCompany();
                case Statistic.Name:
                    return new StatisticName();
                case Statistic.LatestValue:
                    return new StatisticLatestValue();
                case Statistic.RecentChange:
                    return new StatisticRecentChange();
                case Statistic.FundFraction:
                    return new StatisticFundFraction();
                case Statistic.FundCompanyFraction:
                    return new StatisticFundCompanyFraction();
                case Statistic.Profit:
                    return new StatisticProfit();
                case Statistic.IRR3M:
                    return new StatisticIRR3M();
                case Statistic.IRR6M:
                    return new StatisticIRR6M();
                case Statistic.IRR1Y:
                    return new StatisticIRR1Y();
                case Statistic.IRR5Y:
                    return new StatisticIRR5Y();
                case Statistic.IRRTotal:
                    return new StatisticIRRTotal();
                case Statistic.NumberUnits:
                    return new StatisticNumberUnits();
                case Statistic.UnitPrice:
                    return new StatisticUnitPrice();
                case Statistic.Sectors:
                    return new StatisticSectors();
                case Statistic.NumberOfAccounts:
                    return new StatisticNumberOfAccounts();
                default:
                    return new StatisticCompany();
            }
        }

        public static IStatistic Generate(Statistic statTypeToGenerate, IPortfolio portfolio, Account account, TwoName name)
        {
            var stats = Generate(statTypeToGenerate);
            stats.Calculate(portfolio, account, name);
            return stats;
        }

        public static IStatistic Generate(Statistic statTypeToGenerate, IPortfolio portfolio, Totals totals, TwoName name)
        {
            var stats = Generate(statTypeToGenerate);
            stats.Calculate(portfolio, totals, name);
            return stats;
        }
    }
}
