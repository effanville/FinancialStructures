using System.Collections.Generic;
using System.Linq;
using FinancialStructures.Statistics;
using Common.Structure.DisplayClasses;

namespace FinancialStructures.DataExporters.ExportOptions
{
    /// <summary>
    /// Contains options on the display of a portfolio.
    /// </summary>
    public class UserDisplayOptions
    {
        /// <summary>
        /// String to display for showing securities.
        /// </summary>
        public const string ShowSecurities = "ShowSecurites";

        /// <summary>
        /// String to display for showing bank accounts.
        /// </summary>
        public const string ShowBankAccounts = "ShowBankAccounts";

        /// <summary>
        /// String to display for showing sectors.
        /// </summary>
        public const string ShowSectors = "ShowSectors";

        /// <summary>
        /// String to display for showing benchmarks.
        /// </summary>
        public const string ShowBenchmarks = "ShowBenchmarks";

        /// <summary>
        /// Should benchmarks be included in the sector table.
        /// </summary>
        public bool IncludeBenchmarks
        {
            get;
            set;
        }

        /// <summary>
        /// Only display accounts that have non zero current value.
        /// </summary>
        public bool DisplayValueFunds
        {
            get;
            set;
        } = false;

        /// <summary>
        /// Display with spacing in tables.
        /// </summary>
        public bool Spacing
        {
            get;
            set;
        } = false;

        /// <summary>
        /// Display with colours.
        /// </summary>
        public bool Colours
        {
            get;
            set;
        } = false;

        /// <summary>
        /// Options on displaying Securities.
        /// </summary>
        public StatisticTableOptions SecurityDisplayOptions
        {
            get;
        }

        /// <summary>
        /// Options on displaying bank accounts.
        /// </summary>
        public StatisticTableOptions BankAccountDisplayOptions
        {
            get;
        }

        /// <summary>
        /// Options on displaying sectors.
        /// </summary>
        public StatisticTableOptions SectorDisplayOptions
        {
            get;
        }

        /// <summary>
        /// Used for convenience in testing.
        /// </summary>
        internal UserDisplayOptions()
        {
            SecurityDisplayOptions = new StatisticTableOptions();
            BankAccountDisplayOptions = new StatisticTableOptions();
            SectorDisplayOptions = new StatisticTableOptions();
        }

        /// <summary>
        /// Create an instance from all options.
        /// </summary>
        public UserDisplayOptions
            (List<Statistic> securities,
            List<Statistic> bankAccounts,
            List<Statistic> sectors,
            List<Selectable<string>> conditions,
            Statistic securitySortingField = Statistic.Company,
            Statistic bankSortingField = Statistic.Company,
            Statistic sectorSortingField = Statistic.Company,
            SortDirection securitySortDirection = SortDirection.Descending,
            SortDirection bankAccountSortDirection = SortDirection.Descending,
            SortDirection sectorSortDirection = SortDirection.Descending)
        {
            SecurityDisplayOptions = new StatisticTableOptions(SelectableHelpers.GetData(conditions, ShowSecurities), securitySortingField, securitySortDirection, securities);
            BankAccountDisplayOptions = new StatisticTableOptions(SelectableHelpers.GetData(conditions, ShowBankAccounts), bankSortingField, bankAccountSortDirection, bankAccounts);
            SectorDisplayOptions = new StatisticTableOptions(SelectableHelpers.GetData(conditions, ShowSectors), sectorSortingField, sectorSortDirection, sectors);

            IncludeBenchmarks = SelectableHelpers.GetData(conditions, ShowBenchmarks);
            DisplayValueFunds = SelectableHelpers.GetData(conditions, nameof(DisplayValueFunds));
            Spacing = SelectableHelpers.GetData(conditions, nameof(Spacing));
            Colours = SelectableHelpers.GetData(conditions, nameof(Colours));
        }

        /// <summary>
        /// Returns an
        /// </summary>
        /// <returns></returns>
        public static UserDisplayOptions DefaultOptions()
        {
            var options = new List<Selectable<string>>
            {
                new Selectable<string>(nameof(DisplayValueFunds), true),
                new Selectable<string>(nameof(Spacing), true),
                new Selectable<string>(nameof(Colours), true),
                new Selectable<string>(ShowSecurities, true),
                new Selectable<string>(ShowBankAccounts, true),
                new Selectable<string>(ShowSectors, true),
                new Selectable<string>(ShowBenchmarks, true)
            };
            return new UserDisplayOptions(AccountStatisticsHelpers.AllStatistics().ToList(), AccountStatisticsHelpers.DefaultBankAccountStats().ToList(), AccountStatisticsHelpers.DefaultSectorStats().ToList(), options);
        }
    }
}
