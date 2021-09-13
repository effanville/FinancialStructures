using System;
using System.Collections.Generic;
using System.Linq;
using Common.Structure.DisplayClasses;
using Common.UI;
using FinancePortfolioDatabase.GUI.Configuration;
using FinancePortfolioDatabase.GUI.TemplatesAndStyles;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.Statistics;

namespace FinancePortfolioDatabase.GUI.ViewModels.Stats
{
    /// <summary>
    /// View model for the statistics display.
    /// </summary>
    public class StatsViewModel : DataDisplayViewModelBase
    {
        private readonly IConfiguration fUserConfiguration;
        private readonly UiGlobals fUiGlobals;
        private List<AccountStatistics> fStats;

        private bool fDisplayValueFunds = true;

        /// <summary>
        /// Should statistics for funds with non zero value be displayed.
        /// </summary>
        public bool DisplayValueFunds
        {
            get => fDisplayValueFunds;
            set
            {
                SetAndNotify(ref fDisplayValueFunds, value, nameof(DisplayValueFunds));
                UpdateData();
            }
        }

        /// <summary>
        /// The values of the statistics being displayed.
        /// </summary>
        public List<AccountStatistics> Stats
        {
            get => fStats;
            set => SetAndNotify(ref fStats, value, nameof(Stats));
        }

        /// <summary>
        /// The statistics to display.
        /// </summary>
        public List<Selectable<Statistic>> StatisticNames
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StatsViewModel(IPortfolio portfolio, UiStyles styles, UiGlobals globals, IConfiguration userConfiguration, Account account = Account.All, Statistic[] statsToView = null)
            : base(styles, "Statistics", account, portfolio)
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
            Stats = DataStore.GetStats(DataType, DisplayValueFunds, statisticsToDisplay: statsToView);
            fUserConfiguration.StoreConfiguration(this);
        }
    }
}
