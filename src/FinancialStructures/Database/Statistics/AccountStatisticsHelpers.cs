using System;
using System.Collections.Generic;
using System.Linq;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics
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
                if (a.Name.Equals("Totals"))
                {
                    return -1;
                }

                if (b.Name.Equals("Totals"))
                {
                    return 1;
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
        /// Only includes desired statistics. 
        /// </summary>
        public static List<AccountStatistics> Restrict(List<AccountStatistics> stats, IReadOnlyList<Statistic> restrictedStatistics)
        {
            if (stats == null || !stats.Any())
            {
                return stats;
            }

            var currentStats = stats[0].StatisticNames;
            var firstNotSecond = currentStats.Except(restrictedStatistics).ToList();
            var secondNotFirst = restrictedStatistics.Except(currentStats).ToList();
            if (!firstNotSecond.Any() || !secondNotFirst.Any())
            {
                return stats;
            }

            var newList = new List<AccountStatistics>();
            foreach (var stat in stats)
            {
                newList.Add(stat.Restricted(restrictedStatistics));
            }

            return newList;
        }

        /// <summary>
        /// Returns all statistic types currently possible.
        /// </summary>
        public static Statistic[] AllStatistics()
        {
            return Enum.GetValues(typeof(Statistic)).Cast<Statistic>().ToArray();
        }

        /// <summary>
        /// Returns those statistic types suitable for securities.
        /// </summary>
        public static Statistic[] DefaultSecurityStats()
        {
            return new Statistic[]
            {
                Statistic.Company,
                Statistic.Name,
                Statistic.Currency,
                Statistic.LatestValue,
                Statistic.UnitPrice,
                Statistic.NumberUnits,
                Statistic.MeanSharePrice,
                Statistic.RecentChange,
                Statistic.FundFraction,
                Statistic.FundCompanyFraction,
                Statistic.Investment,
                Statistic.Profit,
                Statistic.IRR3M,
                Statistic.IRR6M,
                Statistic.IRR1Y,
                Statistic.IRR5Y,
                Statistic.IRRTotal,
                Statistic.DrawDown,
                Statistic.MDD,
                Statistic.Sectors,
                Statistic.FirstDate,
                Statistic.LastInvestmentDate,
                Statistic.LastPurchaseDate,
                Statistic.LatestDate,
                Statistic.NumberEntries,
                Statistic.EntryYearDensity,
                Statistic.Notes
            };
        }

        /// <summary>
        /// Returns those statistic types suitable for securities.
        /// </summary>
        public static Statistic[] DefaultSecurityCompanyStats()
        {
            return new Statistic[]
            {
                Statistic.Company,
                Statistic.LatestValue,
                Statistic.RecentChange,
                Statistic.FundFraction,
                Statistic.Investment,
                Statistic.Profit,
                Statistic.IRR3M,
                Statistic.IRR6M,
                Statistic.IRR1Y,
                Statistic.IRR5Y,
                Statistic.IRRTotal,
                Statistic.Sectors,
                Statistic.FirstDate,
                Statistic.LatestDate
            };
        }

        /// <summary>
        /// Returns those statistic types suitable for Bank Accounts.
        /// </summary>
        public static Statistic[] DefaultBankAccountStats()
        {
            return new Statistic[]
            {
                Statistic.Company,
                Statistic.Name,
                Statistic.Currency,
                Statistic.LatestValue,
                Statistic.Sectors,
                Statistic.Notes
            };
        }

        /// <summary>
        /// Returns those statistic types suitable for Sectors.
        /// </summary>
        public static Statistic[] DefaultSectorStats()
        {
            return new Statistic[]
            {
                Statistic.Company,
                Statistic.Name,
                Statistic.LatestValue,
                Statistic.RecentChange,
                Statistic.Profit,
                Statistic.IRR3M,
                Statistic.IRR6M,
                Statistic.IRR1Y,
                Statistic.IRR5Y,
                Statistic.IRRTotal,
                Statistic.NumberOfAccounts,
                Statistic.FirstDate,
                Statistic.LatestDate,
                Statistic.Notes
            };
        }


        /// <summary>
        /// Returns those statistic types suitable for Assets.
        /// </summary>
        public static Statistic[] DefaultAssetStats()
        {
            return new Statistic[]
            {
                Statistic.Company,
                Statistic.Name,
                Statistic.LatestValue,
                Statistic.RecentChange,
                Statistic.Investment,
                Statistic.Profit,
                Statistic.Debt,
                Statistic.IRR3M,
                Statistic.IRR6M,
                Statistic.IRR1Y,
                Statistic.IRR5Y,
                Statistic.IRRTotal,
                Statistic.FirstDate,
                Statistic.LatestDate,
                Statistic.Sectors,
                Statistic.Notes,
            };
        }

        /// <summary>
        /// Generates statistic types for database info.
        /// </summary>
        public static Statistic[] DefaultDatabaseStatistics()
        {
            return new Statistic[]
            {
                Statistic.Company,
                Statistic.Name,
                Statistic.FirstDate,
                Statistic.LatestDate,
                Statistic.NumberEntries,
                Statistic.EntryYearDensity
            };
        }
    }
}
