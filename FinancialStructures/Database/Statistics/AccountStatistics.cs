using System;
using System.Collections.Generic;
using System.Linq;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics
{
    /// <summary>
    /// A holder for statistics about a specific account.
    /// </summary>
    public sealed class AccountStatistics : IComparable<AccountStatistics>
    {
        /// <summary>
        /// The type of account referred about.
        /// </summary>
        public Account AccountType
        {
            get;
            private set;
        }

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
        public List<object> StatValuesAsObjects => Statistics.Select(stat => stat.ValueAsObject).ToList();

        /// <summary>
        /// The statistics calculated for this account.
        /// </summary>
        public List<IStatistic> Statistics
        {
            get;
            set;
        } = new List<IStatistic>();

        internal AccountStatistics()
        {
        }

        /// <summary>
        /// Default constructor for statistics for a <see cref="Account"/> object.
        /// </summary>
        public AccountStatistics(IPortfolio portfolio, DateTime dateToCalculate, Account account, TwoName name, Statistic[] statsToGenerate)
        {
            NameData = name;

            foreach (Statistic stat in statsToGenerate)
            {
                IStatistic stats = StatisticFactory.Generate(stat, portfolio, dateToCalculate, account, name);
                Statistics.Add(stats);
            }
        }

        /// <summary>
        /// Default constructor for statistics for a <see cref="Totals"/>
        /// </summary>
        public AccountStatistics(IPortfolio portfolio, DateTime dateToCalculate, Totals total, TwoName name, Statistic[] statsToGenerate)
        {
            NameData = name;

            foreach (Statistic stat in statsToGenerate)
            {
                IStatistic stats = StatisticFactory.Generate(stat, portfolio, dateToCalculate, total, name);
                Statistics.Add(stats);
            }
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
        public int CompareTo(AccountStatistics other)
        {
            return NameData.CompareTo(other.NameData);
        }
    }
}
