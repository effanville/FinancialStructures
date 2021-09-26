using System;
using System.Collections.Generic;
using System.Windows.Input;
using Common.Structure.DisplayClasses;
using Common.Structure.Extensions;
using Common.Structure.FileAccess;
using Common.Structure.Reporting;
using Common.UI;
using Common.UI.Commands;
using Common.UI.Services;
using FinancePortfolioDatabase.GUI.Configuration;
using FinancePortfolioDatabase.GUI.TemplatesAndStyles;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.Database;
using FinancialStructures.DataExporters.Statistics;
using FinancialStructures.Statistics;

namespace FinancePortfolioDatabase.GUI.ViewModels.Stats
{
    /// <summary>
    /// View model for the stats options page.
    /// </summary>
    public class ExportStatsViewModel : DataDisplayViewModelBase
    {
        private readonly Action<object> CloseWindowAction;

        private List<Selectable<string>> fDisplayConditions = new List<Selectable<string>>();

        private const string ShowSecurities = "ShowSecurites";
        private const string ShowBankAccounts = "ShowBankAccounts";

        private const string ShowSectors = "ShowSectors";
        private const string ShowBenchmarks = "ShowBenchmarks";
        private const string ValueFunds = "DisplayValueFunds";
        private const string Colouring = "Colours";
        private const string Spacing = "Spacing";

        /// <summary>
        /// Miscellaneous selections for how the exported file should look.
        /// </summary>
        public List<Selectable<string>> DisplayConditions
        {
            get => fDisplayConditions;
            set => SetAndNotify(ref fDisplayConditions, value, nameof(DisplayConditions));
        }

        private Statistic fSecuritySortingField;

        /// <summary>
        /// The statistic to sort the security data by.
        /// </summary>
        public Statistic SecuritySortingField
        {
            get => fSecuritySortingField;
            set => SetAndNotify(ref fSecuritySortingField, value, nameof(SecuritySortingField));
        }

        private SortDirection fSecurityDirection;

        /// <summary>
        /// The direction to sort the Security data in.
        /// </summary>
        public SortDirection SecurityDirection
        {
            get => fSecurityDirection;
            set => SetAndNotify(ref fSecurityDirection, value, nameof(SecurityDirection));
        }

        private List<Selectable<Statistic>> fSecurityColumnNames = new List<Selectable<Statistic>>();

        /// <summary>
        /// The possible columns for security export, and which ones are selected.
        /// </summary>
        public List<Selectable<Statistic>> SecurityColumnNames
        {
            get => fSecurityColumnNames;
            set => SetAndNotify(ref fSecurityColumnNames, value, nameof(SecurityColumnNames));
        }

        private Statistic fBankSortingField;

        /// <summary>
        /// The statistic to sort the bank account data by.
        /// </summary>
        public Statistic BankSortingField
        {
            get => fBankSortingField;
            set => SetAndNotify(ref fBankSortingField, value, nameof(BankSortingField));
        }

        private SortDirection fBankDirection;

        /// <summary>
        /// The direction to sort the Bank Account data in.
        /// </summary>
        public SortDirection BankDirection
        {
            get => fBankDirection;
            set => SetAndNotify(ref fBankDirection, value, nameof(BankDirection));
        }

        private List<Selectable<Statistic>> fBankColumnNames = new List<Selectable<Statistic>>();

        /// <summary>
        /// The possible columns for bank account export, and which ones are selected.
        /// </summary>
        public List<Selectable<Statistic>> BankColumnNames
        {
            get => fBankColumnNames;
            set => SetAndNotify(ref fBankColumnNames, value, nameof(BankColumnNames));
        }

        private Statistic fSectorSortingField;

        /// <summary>
        /// The statistic to sort the sector data by.
        /// </summary>
        public Statistic SectorSortingField
        {
            get => fSectorSortingField;
            set => SetAndNotify(ref fSectorSortingField, value, nameof(SectorSortingField));
        }


        private SortDirection fSectorDirection;

        /// <summary>
        /// The direction to sort the Sector data in.
        /// </summary>
        public SortDirection SectorDirection
        {
            get => fSectorDirection;
            set => SetAndNotify(ref fSectorDirection, value, nameof(SectorDirection));
        }

        private List<Selectable<Statistic>> fSectorColumnNames = new List<Selectable<Statistic>>();

        /// <summary>
        /// The possible columns for sector export, and which ones are selected.
        /// </summary>
        public List<Selectable<Statistic>> SectorColumnNames
        {
            get => fSectorColumnNames;
            set => SetAndNotify(ref fSectorColumnNames, value, nameof(SectorColumnNames));
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ExportStatsViewModel(UiGlobals globals, UiStyles styles, IConfiguration userConfiguration, IPortfolio portfolio, Action<object> CloseWindow)
            : base(globals, styles, userConfiguration, portfolio, "", Account.All)
        {
            CloseWindowAction = CloseWindow;
            ExportCommand = new RelayCommand(ExecuteExportCommand);
            if (fUserConfiguration.HasLoaded)
            {
                fUserConfiguration.RestoreFromConfiguration(this);
            }
            else
            {
                foreach (Statistic stat in AccountStatisticsHelpers.AllStatistics())
                {
                    SecurityColumnNames.Add(new Selectable<Statistic>(stat, true));
                }

                SecuritySortingField = Statistic.Company;

                foreach (Statistic stat in AccountStatisticsHelpers.DefaultSectorStats())
                {
                    SectorColumnNames.Add(new Selectable<Statistic>(stat, true));
                }

                SectorSortingField = Statistic.Name;

                foreach (Statistic stat in AccountStatisticsHelpers.DefaultBankAccountStats())
                {
                    BankColumnNames.Add(new Selectable<Statistic>(stat, true));
                }

                BankSortingField = Statistic.Company;

                DisplayConditions.Add(new Selectable<string>(ValueFunds, true));
                DisplayConditions.Add(new Selectable<string>(Spacing, true));
                DisplayConditions.Add(new Selectable<string>(Colouring, true));
                DisplayConditions.Add(new Selectable<string>(ShowSecurities, true));
                DisplayConditions.Add(new Selectable<string>(ShowBankAccounts, true));
                DisplayConditions.Add(new Selectable<string>(ShowSectors, true));
                DisplayConditions.Add(new Selectable<string>(ShowBenchmarks, false));

                fUserConfiguration.HasLoaded = true;
                fUserConfiguration.StoreConfiguration(this);
            }
        }

        /// <summary>
        /// Command to instantiate the export of statistics.
        /// </summary>
        public ICommand ExportCommand
        {
            get;
        }

        private void ExecuteExportCommand()
        {
            fUserConfiguration.StoreConfiguration(this);
            FileInteractionResult result = fUiGlobals.FileInteractionService.SaveFile(ExportType.Html.ToString(), DataStore.DatabaseName(fUiGlobals.CurrentFileSystem), DataStore.Directory(fUiGlobals.CurrentFileSystem), "Html Files|*.html|CSV Files|*.csv|All Files|*.*");
            string path = null;

            if (result.Success != null && (bool)result.Success)
            {
                path = result.FilePath;

                List<Statistic> securitySelected = new List<Statistic>();
                foreach (Selectable<Statistic> column in SecurityColumnNames)
                {
                    if (column.Selected || column.Instance == Statistic.Company || column.Instance == Statistic.Name)
                    {
                        securitySelected.Add(column.Instance);
                    }
                }

                List<Statistic> BankSelected = new List<Statistic>();
                foreach (Selectable<Statistic> column in BankColumnNames)
                {
                    if (column.Selected || column.Instance == Statistic.Company || column.Instance == Statistic.Name)
                    {
                        BankSelected.Add(column.Instance);
                    }
                }

                List<Statistic> sectorSelected = new List<Statistic>();
                foreach (Selectable<Statistic> column in SectorColumnNames)
                {
                    if (column.Selected || column.Instance == Statistic.Company || column.Instance == Statistic.Name)
                    {
                        sectorSelected.Add(column.Instance);
                    }
                }

                var settings = new PortfolioStatisticsSettings(
                    SelectableHelpers.GetData(DisplayConditions, ShowBenchmarks),
                    SelectableHelpers.GetData(DisplayConditions, ValueFunds),
                    SelectableHelpers.GetData(DisplayConditions, ShowSecurities),
                    SecuritySortingField,
                    SecurityDirection,
                    securitySelected,
                    SelectableHelpers.GetData(DisplayConditions, ShowBankAccounts),
                    BankSortingField,
                    BankDirection,
                    BankSelected,
                    SelectableHelpers.GetData(DisplayConditions, ShowSectors),
                    SectorSortingField,
                    SectorDirection,
                    sectorSelected);

                PortfolioStatistics stats = new PortfolioStatistics(DataStore, settings, fUiGlobals.CurrentFileSystem);
                string extension = fUiGlobals.CurrentFileSystem.Path.GetExtension(result.FilePath).Trim('.');
                ExportType type = extension.ToEnum<ExportType>();

                stats.ExportToFile(fUiGlobals.CurrentFileSystem, result.FilePath, type, new PortfolioStatisticsExportSettings(SelectableHelpers.GetData(DisplayConditions, Spacing), SelectableHelpers.GetData(DisplayConditions, Colouring)), ReportLogger);

                _ = ReportLogger.LogUseful(ReportType.Information, ReportLocation.StatisticsPage, "Created statistics page");
            }
            else
            {
                _ = ReportLogger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.StatisticsPage, "Was not able to create page in place specified.");
            }

            CloseWindowAction(new HtmlStatsViewerViewModel(Styles, fUiGlobals, path));
        }
    }
}
