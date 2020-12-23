using System;
using System.Collections.Generic;
using System.Linq;
using FinancialStructures.Statistics;
using FinancialStructures.Database.Statistics;
using UICommon.DisplayClasses;
using FinancialStructures.Database;

namespace FinanceViewModels.StatsViewModels
{
    internal class AccountStatisticsViewModel : TabViewModelBase
    {
        private Account fAccount;

        private List<AccountStatistics> fSecuritiesStats;

        public List<AccountStatistics> Stats
        {
            get
            {
                return fSecuritiesStats;
            }
            set
            {
                fSecuritiesStats = value;
                OnPropertyChanged();
            }
        }

        private List<Selectable<Statistic>> fStatisticNames;

        public List<Selectable<Statistic>> StatisticNames
        {
            get
            {
                return fStatisticNames;
            }
            set
            {
                fStatisticNames = value;
            }
        }

        public AccountStatisticsViewModel(IPortfolio portfolio, Account account, bool displayValueFunds)
            : base(portfolio, displayValueFunds)
        {
            fAccount = account;
            Header = $"{account} Statistics";
            StatisticNames = AccountStatisticsHelpers.AllStatistics().Select(stat => new Selectable<Statistic>(stat, true)).ToList();
            StatisticNames.ForEach(stat => stat.SelectedChanged += OnSelectedChanged);
            GenerateStatistics(displayValueFunds);
        }

        /// <inheritdoc/>
        public override void GenerateStatistics(bool displayValueFunds)
        {
            DisplayValueFunds = displayValueFunds;
            Statistic[] statsToView = StatisticNames.Where(stat => stat.Selected).Select(stat => stat.Instance).ToArray();
            Stats = fPortfolio.GetStats(fAccount, displayValueFunds, statisticsToDisplay: statsToView);
        }

        /// <summary>
        /// Update to regenerate the statistics displayed if required.
        /// </summary>
        private void OnSelectedChanged(object sender, EventArgs e)
        {
            GenerateStatistics(DisplayValueFunds);
        }
    }
}
