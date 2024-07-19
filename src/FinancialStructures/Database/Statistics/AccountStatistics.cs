using System;
using System.Collections.Generic;
using System.Linq;

using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics
{
    /// <summary>
    /// A holder for statistics about a specific account.
    /// </summary>
    public sealed class AccountStatistics : IComparable<AccountStatistics>
    {
        /// <summary>
        /// The names associated to these statistics.
        /// </summary>
        public TwoName NameData
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns a list of the statistic values in object form.
        /// </summary>
        public IReadOnlyList<object> StatValuesAsObjects => Statistics.Select(stat => stat.ValueAsObject).ToList();

        /// <summary>
        /// The names of the statistics 
        /// </summary>
        public IReadOnlyList<Statistic> StatisticNames => Statistics.Select(stat => stat.StatType).ToList();

        /// <summary>
        /// The statistics calculated for this account.
        /// </summary>
        public IReadOnlyList<IStatistic> Statistics
        {
            get;
            private set;
        }

        internal AccountStatistics()
        {
        }
        
        /// <summary>
        /// Default constructor for statistics for a <see cref="Account"/> object.
        /// </summary>
        public AccountStatistics(IPortfolio portfolio, DateTime dateToCalculate, IValueList valueList, Statistic[] statsToGenerate)
        {
            NameData = valueList.Names;
            List<IStatistic> statistics = new List<IStatistic>();
            foreach (Statistic stat in statsToGenerate)
            {
                IStatistic stats = StatisticFactory.Generate(stat, valueList, portfolio, dateToCalculate);
                statistics.Add(stats);
            }

            Statistics = statistics;
        }

        /// <summary>
        /// Default constructor for statistics for a <see cref="Totals"/>
        /// </summary>
        public AccountStatistics(IPortfolio portfolio, DateTime dateToCalculate, Totals total, TwoName name, Statistic[] statsToGenerate)
        {
            NameData = name;
            List<IStatistic> statistics = new List<IStatistic>();
            foreach (Statistic stat in statsToGenerate)
            {
                IStatistic stats = StatisticFactory.Generate(stat, portfolio, dateToCalculate, total, name);
                statistics.Add(stats);
            }

            Statistics = statistics;
        }

        /// <summary>
        /// Returns a new Account Statistics object with only the desired statistics.
        /// </summary>
        /// <param name="statsToRestrict"></param>
        /// <returns></returns>
        public AccountStatistics Restricted(IReadOnlyList<Statistic> statsToRestrict)
        {
            AccountStatistics accountStats = new AccountStatistics
            {
                NameData = NameData
            };

            List<IStatistic> statistics = new List<IStatistic>();
            foreach (Statistic stat in statsToRestrict)
            {
                IStatistic statistic = GetStatistic(stat);
                if (statistic != null)
                {
                    statistics.Add(statistic);
                }
            }

            accountStats.Statistics = statistics;
            return accountStats;
        }

        internal IStatistic GetStatistic(Statistic field)
        {
            foreach (IStatistic stat in Statistics)
            {
                if (stat.StatType == field)
                {
                    return stat;
                }
            }

            return null;
        }

        /// <inheritdoc/>
        public int CompareTo(AccountStatistics other) => NameData.CompareTo(other.NameData);

        /// <summary>
        /// Compares to another instance, where the comparison is on the specified
        /// statistic and in the specified direction
        /// </summary>
        public int CompareTo(AccountStatistics other, Statistic compareStatistic, SortDirection sortDirection)
        {
                IStatistic firstField = Statistics.First(stat => stat.StatType == compareStatistic);
                IStatistic secondField = other.Statistics.First(stat => stat.StatType == compareStatistic);

                return sortDirection == SortDirection.Descending 
                    ? secondField.CompareTo(firstField) 
                    : firstField.CompareTo(secondField);
        }
    }
}
