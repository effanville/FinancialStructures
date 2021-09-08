using System.Collections.Generic;
using FinancialStructures.Statistics;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.Database;
using Common.Structure.Reporting;
using Common.UI;
using FinancePortfolioDatabase.GUI.Configuration;
using Common.Structure.DisplayClasses;
using System;
using System.Linq;
using FinancialStructures.Database.Statistics;

namespace FinancePortfolioDatabase.GUI.ViewModels.Stats
{
    /// <summary>
    /// View model for the statistics display.
    /// </summary>
    public class StatsViewModel : DataDisplayViewModelBase
    {
        private readonly IConfiguration fUserConfiguration;
        private readonly UiGlobals fUiGlobals;
        private readonly IReportLogger ReportLogger;
        private readonly Account fAccount;
        private List<AccountStatistics> fSecuritiesStats;

        private bool fDisplayValueFunds = true;
        public bool DisplayValueFunds
        {
            get => fDisplayValueFunds;
            set
            {
                SetAndNotify(ref fDisplayValueFunds, value, nameof(DisplayValueFunds));
                UpdateData();
            }
        }

        public List<AccountStatistics> Stats
        {
            get => fSecuritiesStats;
            set => SetAndNotify(ref fSecuritiesStats, value, nameof(Stats));
        }

        public List<Selectable<Statistic>> StatisticNames
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StatsViewModel(IPortfolio portfolio, IReportLogger reportLogger, UiGlobals globals, IConfiguration userConfiguration, Account account = Account.All, Statistic[] statsToView = null)
            : base("Statistics", account, portfolio)
        {
            StatisticNames = statsToView != null ? statsToView.Select(stat => new Selectable<Statistic>(stat, true)).ToList() : AccountStatisticsHelpers.AllStatistics().Select(stat => new Selectable<Statistic>(stat, true)).ToList();
            StatisticNames.ForEach(stat => stat.SelectedChanged += OnSelectedChanged);
            fUserConfiguration = userConfiguration;
            if (fUserConfiguration.HasLoaded)
            {
                fUserConfiguration.RestoreFromConfiguration(this);
            }

            fUserConfiguration.HasLoaded = true;
            fUiGlobals = globals;
            ReportLogger = reportLogger;
            UpdateData(portfolio);
        }

        /// <summary>
        /// Update to regenerate the statistics displayed if required.
        /// </summary>
        private void OnSelectedChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        /// <inheritdoc/>
        public override void UpdateData(IPortfolio dataToDisplay = null)
        {
            if (dataToDisplay != null)
            {
                base.UpdateData(dataToDisplay);
            }

            Statistic[] statsToView = StatisticNames.Where(stat => stat.Selected).Select(stat => stat.Instance).ToArray();
            Stats = DataStore.GetStats(fAccount, DisplayValueFunds, statisticsToDisplay: statsToView);
            fUserConfiguration.StoreConfiguration(this);
        }
    }
}
