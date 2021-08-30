using System;
using System.Collections.Generic;
using System.Linq;
using FinancialStructures.DataStructures;
using FinancialStructures.Statistics;

namespace FinancePortfolioDatabase.GUI
{
    /// <summary>
    /// Container class for various constant values to be used in the display.
    /// </summary>
    public sealed class DisplayConstants
    {
        /// <summary>
        /// A list of all possible <see cref="TradeType"/> values.
        /// </summary>
        public static List<TradeType> TradeTypes => Enum.GetValues(typeof(TradeType)).Cast<TradeType>().ToList();

        public static List<Statistic> SecurityFieldNames => AccountStatisticsHelpers.AllStatistics().ToList();

        public static List<Statistic> BankFieldNames => AccountStatisticsHelpers.DefaultBankAccountStats().ToList();

        public static List<Statistic> SectorFieldNames => AccountStatisticsHelpers.DefaultSectorStats().ToList();

        public static List<SortDirection> SortDirections => Enum.GetValues(typeof(SortDirection)).Cast<SortDirection>().ToList();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DisplayConstants()
        {
        }
    }
}
