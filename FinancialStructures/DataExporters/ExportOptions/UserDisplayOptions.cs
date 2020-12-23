using System.Collections.Generic;
using FinancialStructures.Statistics;
using StructureCommon.DisplayClasses;

namespace FinancialStructures.DataExporters.ExportOptions
{
    /// <summary>
    /// Contains options on the display of a portfolio.
    /// </summary>
    public class UserDisplayOptions
    {
        public const string ShowSecurities = "ShowSecurites";
        public const string ShowBankAccounts = "ShowBankAccounts";
        public const string ShowSectors = "ShowSectors";

        public bool DisplayValueFunds
        {
            get;
            set;
        } = false;

        public bool Spacing
        {
            get;
            set;
        } = false;

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
        }

        /// <summary>
        /// Create an instance from all options.
        /// </summary>
        public UserDisplayOptions
            (List<Statistic> securities, List<Statistic> bankAccounts, List<Statistic> sectors, List<Selectable<string>> conditions, Statistic securitySortingField = Statistic.Company, Statistic bankSortingField = Statistic.Company, Statistic sectorSortingField = Statistic.Company, SortDirection securitySortDirection = SortDirection.Descending, SortDirection bankAccountSortDirection = SortDirection.Descending, SortDirection sectorSortDirection = SortDirection.Descending)
        {
            SecurityDisplayOptions = new StatisticTableOptions(SelectableHelpers.GetData(conditions, ShowSecurities), securitySortingField, securitySortDirection, securities);
            BankAccountDisplayOptions = new StatisticTableOptions(SelectableHelpers.GetData(conditions, ShowBankAccounts), bankSortingField, bankAccountSortDirection, bankAccounts);
            SectorDisplayOptions = new StatisticTableOptions(SelectableHelpers.GetData(conditions, ShowSectors), sectorSortingField, sectorSortDirection, sectors);

            DisplayValueFunds = SelectableHelpers.GetData(conditions, nameof(DisplayValueFunds));
            Spacing = SelectableHelpers.GetData(conditions, nameof(Spacing));
            Colours = SelectableHelpers.GetData(conditions, nameof(Colours));
        }
    }
}
