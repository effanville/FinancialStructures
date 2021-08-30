using System;
using System.Collections.Generic;
using System.Windows.Input;
using FinancialStructures.Database;
using FinancialStructures.DataExporters;
using FinancialStructures.DataExporters.ExportOptions;
using FinancialStructures.Statistics;
using Common.Structure.DisplayClasses;
using Common.Structure.Extensions;
using Common.Structure.FileAccess;
using Common.Structure.Reporting;
using Common.UI.Commands;
using Common.UI.Interfaces;
using Common.UI.Services;
using Common.UI.ViewModelBases;
using Common.UI;
using FinancePortfolioDatabase.GUI.Configuration;

namespace FinancePortfolioDatabase.GUI.ViewModels.Stats
{
    /// <summary>
    /// View model for the stats options page.
    /// </summary>
    public class StatsOptionsViewModel : PropertyChangedBase
    {
        private readonly IConfiguration fUserConfiguration;
        private readonly IPortfolio Portfolio;

        private List<Selectable<string>> fDisplayConditions = new List<Selectable<string>>();

        public List<Selectable<string>> DisplayConditions
        {
            get => fDisplayConditions;
            set => SetAndNotify(ref fDisplayConditions, value, nameof(DisplayConditions));
        }

        private Statistic fSecuritySortingField;
        public Statistic SecuritySortingField
        {
            get => fSecuritySortingField;
            set => SetAndNotify(ref fSecuritySortingField, value, nameof(SecuritySortingField));
        }

        private SortDirection fSecurityDirection;
        public SortDirection SecurityDirection
        {
            get => fSecurityDirection;
            set => SetAndNotify(ref fSecurityDirection, value, nameof(SecurityDirection));
        }

        private List<Selectable<Statistic>> fSecurityColumnNames = new List<Selectable<Statistic>>();

        public List<Selectable<Statistic>> SecurityColumnNames
        {
            get => fSecurityColumnNames;
            set => SetAndNotify(ref fSecurityColumnNames, value, nameof(SecurityColumnNames));
        }

        private Statistic fBankSortingField;
        public Statistic BankSortingField
        {
            get => fBankSortingField;
            set => SetAndNotify(ref fBankSortingField, value, nameof(BankSortingField));
        }

        private SortDirection fBankDirection;
        public SortDirection BankDirection
        {
            get => fBankDirection;
            set => SetAndNotify(ref fBankDirection, value, nameof(BankDirection));
        }

        private List<Selectable<Statistic>> fBankColumnNames = new List<Selectable<Statistic>>();

        public List<Selectable<Statistic>> BankColumnNames
        {
            get => fBankColumnNames;
            set => SetAndNotify(ref fBankColumnNames, value, nameof(BankColumnNames));
        }

        private Statistic fSectorSortingField;
        public Statistic SectorSortingField
        {
            get => fSectorSortingField;
            set => SetAndNotify(ref fSectorSortingField, value, nameof(SectorSortingField));
        }


        private SortDirection fSectorDirection;
        public SortDirection SectorDirection
        {
            get => fSectorDirection;
            set => SetAndNotify(ref fSectorDirection, value, nameof(SectorDirection));
        }

        private List<Selectable<Statistic>> fSectorColumnNames = new List<Selectable<Statistic>>();

        public List<Selectable<Statistic>> SectorColumnNames
        {
            get => fSectorColumnNames;
            set => SetAndNotify(ref fSectorColumnNames, value, nameof(SectorColumnNames));
        }

        public ICommand ExportCommand
        {
            get;
        }

        private void ExecuteExportCommand(ICloseable window)
        {
            fUserConfiguration.StoreConfiguration(this);
            FileInteractionResult result = fUiGlobals.FileInteractionService.SaveFile(ExportType.Html.ToString(), Portfolio.DatabaseName(fUiGlobals.CurrentFileSystem), Portfolio.Directory(fUiGlobals.CurrentFileSystem), "Html Files|*.html|CSV Files|*.csv|All Files|*.*");
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

                PortfolioStatistics stats = new PortfolioStatistics(Portfolio, options, fUiGlobals.CurrentFileSystem);
                string extension = fUiGlobals.CurrentFileSystem.Path.GetExtension(result.FilePath).Trim('.');
                ExportType type = extension.ToEnum<ExportType>();

                stats.ExportToFile(fUiGlobals.CurrentFileSystem, result.FilePath, type, options, ReportLogger);

                _ = ReportLogger.LogUseful(ReportType.Information, ReportLocation.StatisticsPage, "Created statistics page");
            }
            else
            {
                _ = ReportLogger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.StatisticsPage, "Was not able to create page in place specified.");
            }

            CloseWindowAction(path);
            window.Close();
        }

        private readonly IReportLogger ReportLogger;
        private readonly Action<string> CloseWindowAction;
        private readonly UiGlobals fUiGlobals;

        public StatsOptionsViewModel(IPortfolio portfolio, IReportLogger reportLogger, Action<string> CloseWindow, UiGlobals uiGlobals, IConfiguration userConfiguration)
        {
            fUiGlobals = uiGlobals;
            fUserConfiguration = userConfiguration;
            Portfolio = portfolio;
            ReportLogger = reportLogger;
            CloseWindowAction = CloseWindow;
            ExportCommand = new RelayCommand<ICloseable>(ExecuteExportCommand);
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
            }
        }
    }
}
