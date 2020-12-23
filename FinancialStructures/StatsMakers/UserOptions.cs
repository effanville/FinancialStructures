using System.Collections.Generic;
using FinancialStructures.Statistics;
using FinancialStructures.StatisticStructures;
using StructureCommon.DisplayClasses;

namespace FinancialStructures.StatsMakers
{
    public class UserOptions
    {
        public UserOptions()
        {
        }

        public UserOptions(List<Statistic> securities, List<Statistic> bankaccs, List<Statistic> sectors, List<Selectable<string>> conditions, Statistic securitySortingField = Statistic.Company, Statistic bankSortingField = Statistic.Company, Statistic sectorSortingField = Statistic.Company, SortDirection securitySortDirection = SortDirection.Descending, SortDirection bankAccountSortDirection = SortDirection.Descending, SortDirection sectorSortDirection = SortDirection.Descending)
        {
            SecurityDataToExport = securities;
            BankAccDataToExport = bankaccs;
            SectorDataToExport = sectors;
            BankAccountSortingField = bankSortingField;
            SecuritySortingField = securitySortingField;
            SectorSortingField = sectorSortingField;
            DisplayValueFunds = SelectableHelpers.GetData(conditions, nameof(DisplayValueFunds));
            Spacing = SelectableHelpers.GetData(conditions, nameof(Spacing));
            Colours = SelectableHelpers.GetData(conditions, nameof(Colours));

            ShowSecurites = SelectableHelpers.GetData(conditions, nameof(ShowSecurites));
            ShowBankAccounts = SelectableHelpers.GetData(conditions, nameof(ShowBankAccounts));
            ShowSectors = SelectableHelpers.GetData(conditions, nameof(ShowSectors));

            SecuritySortDirection = securitySortDirection;
            BankSortDirection = bankAccountSortDirection;
            SectorSortDirection = sectorSortDirection;
        }

        public Statistic SecuritySortingField
        {
            get;
        }

        public SortDirection SecuritySortDirection
        {
            get;
        } = SortDirection.Descending;

        public List<Statistic> SecurityDataToExport
        {
            get;
        } = new List<Statistic>();

        public Statistic BankAccountSortingField
        {
            get;
        }

        public SortDirection BankSortDirection
        {
            get;
        } = SortDirection.Descending;

        public List<Statistic> BankAccDataToExport
        {
            get;
        } = new List<Statistic>();

        public Statistic SectorSortingField
        {
            get;
            set;
        }

        public SortDirection SectorSortDirection
        {
            get;
            set;
        } = SortDirection.Descending;

        public List<Statistic> SectorDataToExport
        {
            get;
            set;
        } = new List<Statistic>();

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

        public bool ShowSecurites
        {
            get;
            set;
        } = true;

        public bool ShowBankAccounts
        {
            get;
            set;
        } = true;

        public bool ShowSectors
        {
            get;
            set;
        } = true;
    }
}
