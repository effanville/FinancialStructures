using FinancialStructures.Database;
using FinancialStructures.DataStructures;
using FinancialStructures.DisplayStructures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Media;
using System.Windows.Threading;

namespace FinanceViewModels.StatsViewModels
{
    class StatisticsChartsViewModel : TabViewModelBase
    {
        private Random rnd = new Random();
        private int fHistoryGapDays = 25;
        public int HistoryGapDays
        {
            get { return fHistoryGapDays; }
            set { fHistoryGapDays = value; OnPropertyChanged(); }
        }

        private List<HistoryStatistic> fHistoryStats;
        public List<HistoryStatistic> HistoryStats
        {
            get { return fHistoryStats; }
            set { fHistoryStats = value; OnPropertyChanged(); }
        }

        private List<DailyValuation_Named> fDistributionValues;
        public List<DailyValuation_Named> DistributionValues
        {
            get { return fDistributionValues; }
            set { fDistributionValues = value; OnPropertyChanged(); }
        }


        private List<DailyValuation_Named> fDistributionValues2;
        public List<DailyValuation_Named> DistributionValues2
        {
            get { return fDistributionValues2; }
            set { fDistributionValues2 = value; OnPropertyChanged(); }
        }

        private List<DailyValuation_Named> fDistributionValues3;

        public List<DailyValuation_Named> DistributionValues3
        {
            get { return fDistributionValues3; }
            set { fDistributionValues3 = value; OnPropertyChanged(); }
        }
        private ObservableCollection<LineSeries> fIRRlines = new ObservableCollection<LineSeries>();
        public ObservableCollection<LineSeries> IRRLines 
        { 
            get { return fIRRlines; }
            set { fIRRlines = value; OnPropertyChanged(); }
        }

        public override async void GenerateStatistics(bool displayValueFunds)
        {
            DisplayValueFunds = displayValueFunds;
            HistoryStats = await fPortfolio.GenerateHistoryStats(HistoryGapDays).ConfigureAwait(true);
            DistributionValues = HistoryStats[HistoryStats.Count - 1].SecurityValues;
            DistributionValues2 = HistoryStats[HistoryStats.Count - 1].BankAccValues;
            DistributionValues3 = HistoryStats[HistoryStats.Count - 1].SectorValues;
            UpdateChart();
        }

        public void UpdateChart()
        {
            IRRLines.Clear();
            if (HistoryStats.Count > 1)
            {
                for (int sectorIndex = 0; sectorIndex < HistoryStats[0].SectorCar.Count; sectorIndex++)
                {
                    if (HistoryStats[HistoryStats.Count - 1].SectorValues[sectorIndex].Value > 5000)
                    {
                        var pc = new List<KeyValuePair<DateTime, double>>();
                        for (int time = 0; time < HistoryStats.Count; time++)
                        {
                            pc.Add(new KeyValuePair<DateTime, double>(HistoryStats[time].SectorCar[sectorIndex].Day, HistoryStats[time].SectorCar[sectorIndex].Value));
                        }

                        LineSeries series1 = new LineSeries();

                        series1.DependentValuePath = "Value";
                        series1.IndependentValuePath = "Key";
                        series1.ItemsSource = pc;
                        series1.Title = HistoryStats[0].SectorCar[sectorIndex].Company;
                        Style style = new Style(typeof(DataPoint));
                        Setter st1 = new Setter(DataPoint.TemplateProperty, null);
                        Byte[] b = new Byte[3];
                        rnd.NextBytes(b);
                        Setter back = new Setter(DataPoint.BackgroundProperty, new SolidColorBrush(Color.FromRgb(b[0], b[1], b[2])));
                        style.Setters.Add(st1);
                        style.Setters.Add(back);
                        series1.DataPointStyle = style;
                        IRRLines.Add(series1);
                    }
                }
            }
        }

        public StatisticsChartsViewModel(Portfolio portfolio, bool displayValueFunds)
            : base(portfolio, displayValueFunds)
        {
            Header = "Charts";
            GenerateStatistics(displayValueFunds);
        }
    }
}
