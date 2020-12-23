using System;
using System.Collections.Generic;
using System.Linq;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Statistics
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
        public List<object> StatValuesAsObjects
        {
            get
            {
                return Statistics.Select(stat => stat.ValueAsObject).ToList();
            }
        }

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

        public AccountStatistics(IPortfolio portfolio, Account account, TwoName name, Statistic[] statsToGenerate)
        {
            NameData = name;

            foreach (var stat in statsToGenerate)
            {
                var stats = StatisticFactory.Generate(stat, portfolio, account, name);
                Statistics.Add(stats);
            }
        }

        public AccountStatistics(IPortfolio portfolio, Totals total, TwoName name, Statistic[] statsToGenerate)
        {
            NameData = name;

            foreach (var stat in statsToGenerate)
            {
                var stats = StatisticFactory.Generate(stat, portfolio, total, name);
                Statistics.Add(stats);
            }
        }

        internal IStatistic GetStatistic(Statistic field)
        {
            foreach (var stat in Statistics)
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
