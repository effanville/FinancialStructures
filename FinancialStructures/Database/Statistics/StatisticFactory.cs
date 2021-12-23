using System;
using FinancialStructures.Database.Statistics.Implementation;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics
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
                case Statistic.AccountType:
                    return new StatisticAccountType();
                case Statistic.Company:
                    return new StatisticCompany();
                case Statistic.Name:
                    return new StatisticName();
                case Statistic.Currency:
                    return new StatisticCurrency();
                case Statistic.LatestValue:
                    return new StatisticLatestValue();
                case Statistic.RecentChange:
                    return new StatisticRecentChange();
                case Statistic.FundFraction:
                    return new StatisticFundFraction();
                case Statistic.FundCompanyFraction:
                    return new StatisticFundCompanyFraction();
                case Statistic.Investment:
                    return new StatisticInvestment();
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
                case Statistic.DrawDown:
                    return new StatisticDrawDown();
                case Statistic.MDD:
                    return new StatisticMDD();
                case Statistic.IRRTotal:
                    return new StatisticIRRTotal();
                case Statistic.NumberUnits:
                    return new StatisticNumberUnits();
                case Statistic.UnitPrice:
                    return new StatisticUnitPrice();
                case Statistic.MeanSharePrice:
                    return new StatisticMeanSharePrice();
                case Statistic.Sectors:
                    return new StatisticSectors();
                case Statistic.NumberOfAccounts:
                    return new StatisticNumberOfAccounts();
                case Statistic.FirstDate:
                    return new StatisticFirstDate();
                case Statistic.LastInvestmentDate:
                    return new StatisticLastInvestmentDate();
                case Statistic.LastPurchaseDate:
                    return new StatisticLastPurchaseDate();
                case Statistic.LatestDate:
                    return new StatisticLatestDate();
                case Statistic.NumberEntries:
                    return new StatisticNumberEntries();
                case Statistic.EntryYearDensity:
                    return new StatisticEntryYearDensity();
                case Statistic.Notes:
                    return new StatisticNotes();
                default:
                    return new StatisticCompany();
            }
        }

        /// <summary>
        /// Generates a statistic class from the specific type enum.
        /// </summary>
        /// <param name="statTypeToGenerate">The <see cref="Statistic"/> to generate.</param>
        /// <param name="portfolio">The portfolio to generate values from.</param>
        /// <param name="account">The Account type to generate statistics for.</param>
        /// <param name="name">A name to generate statistics with.</param>
        /// <returns>A statistic with the relevant type and no value set.</returns>
        public static IStatistic Generate(Statistic statTypeToGenerate, IPortfolio portfolio, Account account, TwoName name)
        {
            IStatistic stats = Generate(statTypeToGenerate);
            stats.Calculate(portfolio, DateTime.Today, account, name);
            return stats;
        }

        /// <summary>
        /// Generates a statistic class from the specific type enum.
        /// </summary>
        /// <param name="statTypeToGenerate">The <see cref="Statistic"/> to generate.</param>
        /// <param name="portfolio">The portfolio to generate values from.</param>
        /// <param name="dateToCalculate">The date to calculate the stats on.</param>
        /// <param name="account">The Account type to generate statistics for.</param>
        /// <param name="name">A name to generate statistics with.</param>
        /// <returns>A statistic with the relevant type and no value set.</returns>
        public static IStatistic Generate(Statistic statTypeToGenerate, IPortfolio portfolio, DateTime dateToCalculate, Account account, TwoName name)
        {
            IStatistic stats = Generate(statTypeToGenerate);
            stats.Calculate(portfolio, dateToCalculate, account, name);
            return stats;
        }

        /// <summary>
        /// Generates a statistic class from the specific type enum.
        /// </summary>
        /// <param name="statTypeToGenerate">The <see cref="Statistic"/> to generate.</param>
        /// <param name="portfolio">The portfolio to generate values from.</param>
        /// <param name="totals">The totals type to generate statistics for.</param>
        /// <param name="name">A name to generate statistics with.</param>
        /// <returns>A statistic with the relevant type and no value set.</returns>
        public static IStatistic Generate(Statistic statTypeToGenerate, IPortfolio portfolio, Totals totals, TwoName name)
        {
            IStatistic stats = Generate(statTypeToGenerate);
            stats.Calculate(portfolio, DateTime.Today, totals, name);
            return stats;
        }

        /// <summary>
        /// Generates a statistic class from the specific type enum.
        /// </summary>
        /// <param name="statTypeToGenerate">The <see cref="Statistic"/> to generate.</param>
        /// <param name="portfolio">The portfolio to generate values from.</param>
        /// <param name="dateToCalculate">The date to calculate the stats on.</param>
        /// <param name="totals">The totals type to generate statistics for.</param>
        /// <param name="name">A name to generate statistics with.</param>
        /// <returns>A statistic with the relevant type and no value set.</returns>
        public static IStatistic Generate(Statistic statTypeToGenerate, IPortfolio portfolio, DateTime dateToCalculate, Totals totals, TwoName name)
        {
            IStatistic stats = Generate(statTypeToGenerate);
            stats.Calculate(portfolio, dateToCalculate, totals, name);
            return stats;
        }
    }
}
