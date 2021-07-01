using System;
using System.Collections.Generic;
using System.Linq;
using FinancialStructures.DataExporters.ExportOptions;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Statistics
{
    /// <summary>
    /// Contains general helper methods for account statistics.
    /// </summary>
    public static class AccountStatisticsHelpers
    {
        /// <summary>
        /// Provides a sorter for the statistics.
        /// </summary>
        public static Comparison<AccountStatistics> Comparer(Statistic sortField, SortDirection direction)
        {
            if (sortField == Statistic.Company)
            {
                if (direction == SortDirection.Descending)
                {
                    return (a, b) => NameComparison(a.NameData, b.NameData);
                }
                else
                {
                    return (a, b) => NameComparison(b.NameData, a.NameData);
                }
            }
            if (sortField == Statistic.Name)
            {
                if (direction == SortDirection.Descending)

                {
                    return (a, b) => b.NameData.Name.CompareTo(a.NameData.Name);
                }
                else
                {
                    return (a, b) => a.NameData.Name.CompareTo(b.NameData.Name);
                }
            }

            if (direction == SortDirection.Descending)
            {
                return (a, b) => b.GetStatistic(sortField).CompareTo(a.GetStatistic(sortField));
            }

            return (a, b) => a.GetStatistic(sortField).CompareTo(b.GetStatistic(sortField));
        }

        private static int NameComparison(TwoName a, TwoName b)
        {
            if (a.Company.Equals(b.Company))
            {
                if (a.Name == "Totals")
                {
                    return a.Company.CompareTo(b.Company);
                }

                if (b.Name == "Totals")
                {
                    return a.Company.CompareTo(b.Company);
                }
            }
            return b.CompareTo(a);
        }

        /// <summary>
        /// Sorts the list of statistics based upon the field to sort and the direction to sort by.
        /// </summary>
        public static void Sort(this List<AccountStatistics> stats, Statistic sortField, SortDirection direction)
        {
            stats.Sort(Comparer(sortField, direction));
        }

        /// <summary>
        /// Sorts the list of statistics based upon the options provided.
        /// </summary>
        public static void Sort(this List<AccountStatistics> stats, StatisticTableOptions displayOptions)
        {
            stats.Sort(Comparer(displayOptions.SortingField, displayOptions.SortingDirection));
        }

        /// <summary>
        /// Returns all statistic types currently possible.
        /// </summary>
        public static Statistic[] AllStatistics()
        {
            return Enum.GetValues(typeof(Statistic)).Cast<Statistic>().ToArray();
        }

        /// <summary>
        /// Returns those statistic types suitable for Bank Accounts.
        /// </summary>
        public static Statistic[] DefaultBankAccountStats()
        {
            return new Statistic[] { Statistic.Company, Statistic.Name, Statistic.LatestValue, Statistic.Notes };
        }

        /// <summary>
        /// Generates statistic types for database info.
        /// </summary>
        public static Statistic[] DefaultDatabaseStatistics()
        {
            return new Statistic[] { Statistic.Company, Statistic.Name, Statistic.FirstDate, Statistic.LatestDate, Statistic.NumberEntries, Statistic.EntryYearDensity };
        }
    }
}
