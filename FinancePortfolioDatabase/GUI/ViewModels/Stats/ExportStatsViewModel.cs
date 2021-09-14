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
using FinancialStructures.DataExporters;
using FinancialStructures.DataExporters.ExportOptions;
using FinancialStructures.Statistics;

namespace FinancePortfolioDatabase.GUI.ViewModels.Stats
{
    /// <summary>
    /// View model for the stats options page.
    /// </summary>
    public class ExportStatsViewModel : DataDisplayViewModelBase
    {
        private readonly IConfiguration fUserConfiguration;
        private IReportLogger ReportLogger => fUiGlobals.ReportLogger;
        private readonly Action<object> CloseWindowAction;
        private readonly UiGlobals fUiGlobals;

        private List<Selectable<string>> fDisplayConditions = new List<Selectable<string>>();

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
        public ExportStatsViewModel(IPortfolio portfolio, Action<object> CloseWindow, UiStyles styles, UiGlobals uiGlobals, IConfiguration userConfiguration)
            : base(styles, "", Account.All, portfolio)
        {
            fUiGlobals = uiGlobals;
            fUserConfiguration = userConfiguration;
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

                DisplayConditions.Add(new Selectable<string>("DisplayValueFunds", true));
                DisplayConditions.Add(new Selectable<string>("Spacing", true));
                DisplayConditions.Add(new Selectable<string>("Colours", true));
                DisplayConditions.Add(new Selectable<string>(UserDisplayOptions.ShowSecurities, true));
                DisplayConditions.Add(new Selectable<string>(UserDisplayOptions.ShowBankAccounts, true));
                DisplayConditions.Add(new Selectable<string>(UserDisplayOptions.ShowSectors, true));
                DisplayConditions.Add(new Selectable<string>(UserDisplayOptions.ShowBenchmarks, false));

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

                UserDisplayOptions options = new UserDisplayOptions(securitySelected, BankSelected, sectorSelected, DisplayConditions, SecuritySortingField, BankSortingField, SectorSortingField, SecurityDirection, BankDirection, SectorDirection);

                PortfolioStatistics stats = new PortfolioStatistics(DataStore, options, fUiGlobals.CurrentFileSystem);
                string extension = fUiGlobals.CurrentFileSystem.Path.GetExtension(result.FilePath).Trim('.');
                ExportType type = extension.ToEnum<ExportType>();

                stats.ExportToFile(fUiGlobals.CurrentFileSystem, result.FilePath, type, options, ReportLogger);

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
