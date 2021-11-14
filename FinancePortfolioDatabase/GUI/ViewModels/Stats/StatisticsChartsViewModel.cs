using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Media;
using FinancePortfolioDatabase.GUI.TemplatesAndStyles;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.Database;
using FinancialStructures.Database.Export.History;

namespace FinancePortfolioDatabase.GUI.ViewModels.Stats
{
    internal class StatisticsChartsViewModel : DataDisplayViewModelBase
    {
        private int fHistoryGapDays = 25;
        public int HistoryGapDays
        {
            get => fHistoryGapDays;
            set => SetAndNotify(ref fHistoryGapDays, value, nameof(HistoryGapDays));
        }

        private List<PortfolioDaySnapshot> fHistoryStats;
        public List<PortfolioDaySnapshot> HistoryStats
        {
            get => fHistoryStats;
            set => SetAndNotify(ref fHistoryStats, value, nameof(HistoryStats));
        }

        private Dictionary<string, decimal> fDistributionValues;
        public Dictionary<string, decimal> SecurityValues
        {
            get => fDistributionValues;
            set
            {
                fDistributionValues = value;
                OnPropertyChanged();
            }
        }


        private Dictionary<string, decimal> fDistributionValues2;
        public Dictionary<string, decimal> BankAccountValues
        {
            get => fDistributionValues2;
            set => SetAndNotify(ref fDistributionValues2, value, nameof(BankAccountValues));
        }

        private Dictionary<string, decimal> fDistributionValues3;

        public Dictionary<string, decimal> SectorValues
        {
            get => fDistributionValues3;
            set => SetAndNotify(ref fDistributionValues3, value, nameof(SectorValues));
        }

        private ObservableCollection<LineSeries> fIRRlines = new ObservableCollection<LineSeries>();
        public ObservableCollection<LineSeries> IRRLines
        {
            get => fIRRlines;
            set => SetAndNotify(ref fIRRlines, value, nameof(IRRLines));
        }

        public override void UpdateData(IPortfolio portfolio)
        {
            if (portfolio != null)
            {
                base.UpdateData(portfolio);
            }

            PortfolioHistory history = new PortfolioHistory(DataStore, new PortfolioHistorySettings(20, generateSecurityRates: false, generateSectorRates: true));
            HistoryStats = history.Snapshots;
            SecurityValues = HistoryStats[HistoryStats.Count - 1].SecurityValues.Where(x => x.Value > 0).ToDictionary(x => x.Key, x => x.Value);
            BankAccountValues = HistoryStats[HistoryStats.Count - 1].BankAccValues.Where(x => x.Value > 0).ToDictionary(x => x.Key, x => x.Value);
            SectorValues = HistoryStats[HistoryStats.Count - 1].SectorValues.Where(x => x.Value > 0).ToDictionary(x => x.Key, x => x.Value);

            UpdateChart();
        }

        public void UpdateChart()
        {
            var rnd = new Random(12345);
            IRRLines = null;
            ObservableCollection<LineSeries> newValues = new ObservableCollection<LineSeries>();
            if (HistoryStats.Count > 1)
            {
                IReadOnlyList<string> sectorNames = DataStore.Sectors(Account.Security);
                foreach (string name in sectorNames)
                {
                    decimal total = HistoryStats[HistoryStats.Count - 1].SecurityValue;
                    if (HistoryStats[HistoryStats.Count - 1].SectorValues[name] > 0.1m * total)
                    {
                        List<KeyValuePair<DateTime, double>> pc = new List<KeyValuePair<DateTime, double>>();
                        for (int time = 0; time < HistoryStats.Count; time++)
                        {
                            pc.Add(new KeyValuePair<DateTime, double>(HistoryStats[time].Date, HistoryStats[time].CurrentSectorTotalCar[name]));
                        }

                        LineSeries series1 = new LineSeries
                        {
                            DependentValuePath = "Value",
                            IndependentValuePath = "Key",
                            ItemsSource = pc,
                            Title = name
                        };
                        Style style = new Style(typeof(DataPoint));
                        Setter st1 = new Setter(DataPoint.TemplateProperty, null);
                        byte[] b = new byte[3];
                        rnd.NextBytes(b);
                        Setter back = new Setter(DataPoint.BackgroundProperty, new SolidColorBrush(Color.FromRgb(b[0], b[1], b[2])));
                        style.Setters.Add(st1);
                        style.Setters.Add(back);
                        series1.DataPointStyle = style;
                        newValues.Add(series1);
                    }
                }

                IRRLines = newValues;
            }
        }

        public StatisticsChartsViewModel(IPortfolio portfolio, UiStyles styles)
            : base(null, styles, portfolio, "Charts", Account.All)
        {
            UpdateData(portfolio);
        }
    }
}
