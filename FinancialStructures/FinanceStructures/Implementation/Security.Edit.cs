using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Common.Structure.DataStructures;
using Common.Structure.Extensions;
using Common.Structure.Reporting;
using FinancialStructures.DataStructures;

namespace FinancialStructures.FinanceStructures.Implementation
{
    /// <summary>
    /// Contains data editing of a security class.
    /// </summary>
    public partial class Security
    {
        /// <inheritdoc/>
        public override bool TryEditData(DateTime oldDate, DateTime date, double value, IReportLogger reportLogger = null)
        {
            return AddOrEditData(UnitPrice, oldDate, date, value, reportLogger) & EnsureDataConsistency(reportLogger);
        }

        /// <inheritdoc/>
        public override void SetData(DateTime date, double value, IReportLogger loggerLogger = null)
        {
            _ = AddOrEditData(UnitPrice, date, date, value, loggerLogger) & EnsureDataConsistency(loggerLogger);
        }

        /// <inheritdoc/>
        public bool AddOrEditData(DateTime oldDate, DateTime date, double unitPrice, double shares, double investment = 0, SecurityTrade trade = null, IReportLogger reportLogger = null)
        {
            bool editUnitPrice = AddOrEditData(UnitPrice, oldDate, date, unitPrice, reportLogger);
            bool editShares = AddOrEditData(Shares, oldDate, date, shares, reportLogger);
            bool editInvestments = AddOrEditData(Investments, oldDate, date, investment, reportLogger);
            if (trade != null)
            {
                AddOrEditTrade(oldDate, trade);
            }

            return editUnitPrice & editShares & editInvestments && EnsureDataConsistency(reportLogger);
        }

        /// <inheritdoc/>
        public bool TryAddOrEditTradeData(SecurityTrade oldTrade, SecurityTrade newTrade, IReportLogger reportLogger = null)
        {
            AddOrEditTrade(oldTrade.Day, newTrade);
            _ = EnsureDataConsistency(reportLogger);
            OnDataEdit(this, new EventArgs());
            return true;
        }

        private void AddOrEditTrade(DateTime oldDate, SecurityTrade trade)
        {
            lock (TradesLock)
            {
                if (!SecurityTrades.Any(existingTrade => existingTrade.Day.Equals(oldDate)))
                {
                    SecurityTrades.Add(trade);
                }
                else
                {
                    foreach (var tradeVal in SecurityTrades)
                    {
                        if (tradeVal.Day.Equals(oldDate))
                        {
                            tradeVal.Day = trade.Day;
                            tradeVal.NumberShares = trade.NumberShares;
                            tradeVal.UnitPrice = trade.UnitPrice;
                            tradeVal.TradeCosts = trade.TradeCosts;
                            tradeVal.TradeType = trade.TradeType;
                        }
                    }
                }
                OnDataEdit(SecurityTrades, new EventArgs());
                SecurityTrades.Sort();
            }
        }

        private static bool AddOrEditData(TimeList list, DateTime oldDate, DateTime date, double value, IReportLogger reportLogger = null)
        {
            if (list.ValueExists(oldDate, out _))
            {
                return list.TryEditData(oldDate, date, value, reportLogger);
            }

            list.SetData(date, value, reportLogger);
            return true;
        }

        /// <inheritdoc/>
        public override List<object> CreateDataFromCsv(List<string[]> valuationsToRead, IReportLogger reportLogger = null)
        {
            List<object> dailyValuations = new List<object>();
            foreach (string[] dayValuation in valuationsToRead)
            {
                if (dayValuation.Length != 4)
                {
                    _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Loading, "Line in Csv file has incomplete data.");
                    break;
                }

                SecurityDayData line = new SecurityDayData(DateTime.Parse(dayValuation[0], CultureInfo.InvariantCulture), double.Parse(dayValuation[1], CultureInfo.InvariantCulture), double.Parse(dayValuation[2], CultureInfo.InvariantCulture), double.Parse(dayValuation[3], CultureInfo.InvariantCulture));
                dailyValuations.Add(line);
            }

            return dailyValuations;
        }

        /// <inheritdoc/>
        public override void WriteDataToCsv(TextWriter writer, IReportLogger reportLogger)
        {
            foreach (SecurityDayData value in GetDataForDisplay())
            {
                writer.WriteLine(value.ToString());
            }
        }

        /// <summary>
        /// Tries to delete the data. If it can, it deletes all data specified, then returns true only if all data has been successfully deleted.
        /// </summary>
        public override bool TryDeleteData(DateTime date, IReportLogger reportLogger = null)
        {
            return UnitPrice.TryDeleteValue(date, reportLogger)
                & Shares.TryDeleteValue(date, reportLogger)
                & Investments.TryDeleteValue(date, reportLogger)
                && EnsureDataConsistency(reportLogger);
        }

        /// <inheritdoc/>
        public bool TryDeleteTradeData(DateTime date, IReportLogger reportLogger = null)
        {
            bool edited = SecurityTrades.RemoveAll(trade => trade.Day.Equals(date)) != 0 && EnsureDataConsistency(reportLogger);
            if (edited)
            {
                OnDataEdit(SecurityTrades, new EventArgs());
            }

            return edited;
        }

        /// <inheritdoc/>
        public void CleanData()
        {
            Shares.CleanValues();
            Investments.CleanValues(0.0);
        }

        /// <summary>
        /// Upon a new/edit trade, one needs to recompute the values of the investments
        /// One should not change Inv = 0 or Inv > 0  to ensure that dividend reivestments are not accidentally included in a new investment.
        /// This though causes a problem if a value is deleted.
        /// </summary>
        internal bool EnsureDataConsistency(IReportLogger reportLogger = null)
        {
            RemoveEventListening();
            CleanData();
            for (int index = 0; index < SecurityTrades.Count; index++)
            {
                // When a trade is present, number of shares bought/sold should correspond to share number difference before
                // and after.
                // Investment on that day should correspond also.
                SecurityTrade trade = SecurityTrades[index];

                double sign = trade.TradeType.Sign();
                if (trade.TradeType != TradeType.CashPayout)
                {
                    DailyValuation sharesPreviousValue = Shares.ValueBefore(trade.Day) ?? new DailyValuation(DateTime.Today, 0);
                    bool hasShareValue = Shares.TryGetValue(trade.Day, out double shareValue);
                    double expectedNumberShares = sharesPreviousValue.Value + sign * trade.NumberShares;
                    if ((hasShareValue && !Equals(shareValue, expectedNumberShares, 1e-4)) || !hasShareValue)
                    {
                        Shares.SetData(trade.Day, expectedNumberShares.Truncate(4));
                    }
                }

                Investments.SetData(trade.Day, sign * trade.TotalCost, reportLogger);
            }

            // now cycle through Investments removing values that no longer have trades.
            for (int index = 0; index < Investments.Count(); index++)
            {
                DailyValuation investmentValue = Investments[index];
                if (!SecurityTrades.Any(trade => trade.Day == investmentValue.Day))
                {
                    if (Investments.TryDeleteValue(investmentValue.Day, reportLogger))
                    {
                        index--;
                    }
                }
            }

            // now cycle through shares removing values that no longer have trades.
            for (int index = 0; index < Shares.Count(); index++)
            {
                DailyValuation shareValue = Shares[index];
                if (!SecurityTrades.Any(trade => trade.Day == shareValue.Day))
                {
                    if (Shares.TryDeleteValue(shareValue.Day, reportLogger))
                    {
                        index--;
                    }
                }
            }
            SetupEventListening();
            return true;
        }

        /// <summary>
        /// Upon a load of security from file, one needs to recompute the values of the investments
        /// One should not change Inv = 0 or Inv > 0  to ensure that dividend reivestments are not accidentally included in a new investment.
        /// This though causes a problem if a value is deleted.
        /// One adds new trades here if trades do not exist to deal with migrating from an old xml form.
        /// </summary>
        internal void EnsureOnLoadDataConsistency(IReportLogger reportLogger = null)
        {
            RemoveEventListening();
            CleanData();
            for (int index = 0; index < SecurityTrades.Count; index++)
            {
                // When a trade is present, number of shares bought/sold should correspond to share number difference before
                // and after.
                // Investment on that day should correspond also.
                SecurityTrade trade = SecurityTrades[index];
                double sign = trade.TradeType.Sign();

                if (trade.TradeType != TradeType.CashPayout)
                {
                    if (trade.TradeType == TradeType.Sell)
                    {
                        trade.NumberShares = Math.Abs(trade.NumberShares);
                    }

                    DailyValuation sharesPreviousValue = Shares.ValueBefore(trade.Day) ?? new DailyValuation(DateTime.Today, 0);
                    bool hasShareValue = Shares.TryGetValue(trade.Day, out double shareValue);

                    double expectedNumberShares = sharesPreviousValue.Value + sign * trade.NumberShares;
                    if ((hasShareValue && !Equals(shareValue, expectedNumberShares, 1e-4)) || !hasShareValue)
                    {
                        Shares.SetData(trade.Day, expectedNumberShares.Truncate(4));
                    }
                }

                Investments.SetData(trade.Day, sign * trade.TotalCost, reportLogger);
            }

            for (int index = 0; index < Investments.Count(); index++)
            {
                DailyValuation investmentValue = Investments[index];
                if (investmentValue.Value != 0)
                {
                    if (!SecurityTrades.Any(trade => trade.Day == investmentValue.Day))
                    {
                        DailyValuation sharesCurrentValue = Shares.ValueOnOrBefore(investmentValue.Day);
                        DailyValuation sharesPreviousValue = Shares.ValueBefore(investmentValue.Day) ?? new DailyValuation(DateTime.Today, 0);
                        if (sharesCurrentValue != null)
                        {
                            double numShares = sharesCurrentValue.Value - sharesPreviousValue.Value;
                            double unitPrice = UnitPrice.ValueOnOrBefore(investmentValue.Day).Value;
                            double value = numShares * unitPrice;
                            Investments.SetData(investmentValue.Day, value, reportLogger);
                            TradeType trade = value > 0 ? TradeType.Buy : TradeType.Sell;
                            SecurityTrades.Add(new SecurityTrade(trade, Names, investmentValue.Day, Math.Abs(numShares), unitPrice, 0.0));
                        }
                    }
                }
                if (investmentValue.Value == 0)
                {
                    if (Investments.TryDeleteValue(investmentValue.Day, reportLogger))
                    {
                        index--;
                    }
                }
            }

            double previousNumShares = 0.0;
            for (int index = 0; index < Shares.Count(); index++)
            {
                DailyValuation shareValue = Shares[index];
                Shares.SetData(shareValue.Day, shareValue.Value.Truncate(4));
                double shareDiff = shareValue.Value - previousNumShares;
                if (shareDiff != 0.0)
                {
                    if (!SecurityTrades.Any(trade => trade.Day == shareValue.Day))
                    {
                        SecurityTrades.Add(new SecurityTrade(TradeType.Dividend, Names, shareValue.Day, shareDiff, UnitPrice.ValueOnOrBefore(shareValue.Day).Value, 0.0));
                    }
                }
                previousNumShares = shareValue.Value;
            }

            SetupEventListening();
        }

        private static bool Equals(double a, double b, double tol)
        {
            if (Math.Abs(a - b) < tol)
            {
                return true;
            }

            return false;
        }
    }
}
