using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using Common.Structure.DataStructures;
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
        public override bool TryEditData(DateTime oldDate, DateTime newDate, decimal value, IReportLogger logger = null)
        {
            bool edited = AddOrEditData(UnitPrice, oldDate, newDate, value, logger);
            EnsureDataConsistency(logger);
            return edited;
        }

        /// <inheritdoc/>
        public override void SetData(DateTime date, decimal value, IReportLogger logger = null)
        {
            _ = AddOrEditData(UnitPrice, date, date, value, logger);
            EnsureDataConsistency(logger);
        }

        /// <inheritdoc/>
        public bool AddOrEditData(DateTime oldDate, DateTime newDate, decimal unitPrice, decimal shares, decimal investment = 0, SecurityTrade trade = null, IReportLogger reportLogger = null)
        {
            bool editUnitPrice = AddOrEditData(UnitPrice, oldDate, newDate, unitPrice, reportLogger);
            bool editShares = AddOrEditData(Shares, oldDate, newDate, shares, reportLogger);
            bool editInvestments = AddOrEditData(Investments, oldDate, newDate, investment, reportLogger);
            if (trade != null)
            {
                AddOrEditTrade(oldDate, trade);
            }

            EnsureDataConsistency(reportLogger);
            return editUnitPrice & editShares & editInvestments;
        }

        /// <inheritdoc/>
        public bool TryAddOrEditTradeData(SecurityTrade oldTrade, SecurityTrade newTrade, IReportLogger reportLogger = null)
        {
            AddOrEditTrade(oldTrade.Day, newTrade);
            EnsureDataConsistency(reportLogger);
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

        private static bool AddOrEditData(TimeList list, DateTime oldDate, DateTime date, decimal value, IReportLogger reportLogger = null)
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

                SecurityDayData line = new SecurityDayData(
                    DateTime.Parse(dayValuation[0], CultureInfo.InvariantCulture),
                    decimal.Parse(dayValuation[1], CultureInfo.InvariantCulture),
                    decimal.Parse(dayValuation[2], CultureInfo.InvariantCulture),
                    decimal.Parse(dayValuation[3], CultureInfo.InvariantCulture));
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
            bool unitDel = UnitPrice.TryDeleteValue(date, reportLogger);
            bool sharesDel = Shares.TryDeleteValue(date, reportLogger);
            bool invDel = Investments.TryDeleteValue(date, reportLogger);
            EnsureDataConsistency(reportLogger);
            return unitDel & sharesDel & invDel;
        }

        /// <inheritdoc/>
        public bool TryDeleteTradeData(DateTime date, IReportLogger reportLogger = null)
        {
            bool edited = SecurityTrades.RemoveAll(trade => trade.Day.Equals(date)) != 0;
            EnsureDataConsistency(reportLogger);
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
        /// Upon a new/edit/Delete trade, one needs to recompute the values of the investments for that trade.
        /// </summary>
        internal void EnsureDataConsistency(IReportLogger reportLogger = null)
        {
            RemoveEventListening();
            CleanData();
            // When a trade is present, number of shares bought/sold should correspond to share number difference before
            // and after.
            // Investment on that day should correspond also.
			
            // First remove all share values that dont have a trade value.
			// Do this first to ensure that share totals when editing trades are correct.
            for (int index = 0; index < Shares.Count(); index++)
            {
                DailyValuation shareValue = Shares[index];
                if (!SecurityTrades.Any(trade => trade.Day.Equals(shareValue.Day)))
                {
                    if (Shares.TryDeleteValue(shareValue.Day, reportLogger))
                    {
                        index--;
                    }
                }
            }

            // Cycle through all trades as trade can impact later share numbers.
            // Requires trades to be sorted in date order. (this should be a no-op)
            SecurityTrades.Sort();
            foreach (SecurityTrade trade in SecurityTrades)
            {
                if (trade != null)
                {
                    double sign = trade.TradeType.Sign();

                    // if trade should alter number of shares, then alter
                    // if it shouldnt then remove the shares.
                    if (trade.TradeType.IsShareNumberAlteringTradeType())
                    {
                        DailyValuation sharesPreviousValue = Shares.ValueBefore(trade.Day) ?? new DailyValuation(trade.Day, 0);
                        bool hasShareValue = Shares.TryGetValue(trade.Day, out double shareValue);
                        double expectedNumberShares = sharesPreviousValue.Value + sign * trade.NumberShares;
                        if ((hasShareValue && !Equals(shareValue, expectedNumberShares, 1e-4)) || !hasShareValue)
                        {
                            Shares.SetData(trade.Day, expectedNumberShares);
                        }
                    }
                    else
                    {
                        _ = Shares.TryDeleteValue(trade.Day, reportLogger);
                    }

                    // if trade should have investment value, then set the value, if it
                    // shouldnt have value then remove.
                    if (trade.TradeType.IsInvestmentTradeType())
                    {
                        Investments.SetData(trade.Day, sign * trade.TotalCost, reportLogger);
                    }
                    else
                    {
                        _ = Investments.TryDeleteValue(trade.Day, reportLogger);
                    }
                }
            }

            // now cycle through Investments removing values that no longer have trades.
            for (int index = 0; index < Investments.Count(); index++)
            {
                DailyValuation investmentValue = Investments[index];
                if (!SecurityTrades.Any(trade => trade.Day.Equals(investmentValue.Day)))
                {
                    if (Investments.TryDeleteValue(investmentValue.Day, reportLogger))
                    {
                        index--;
                    }
                }
            }

            SetupEventListening();
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
                decimal sign = trade.TradeType.Sign();

                if (trade.TradeType.IsShareNumberAlteringTradeType())
                {
                    if (trade.TradeType == TradeType.Sell)
                    {
                        trade.NumberShares = Math.Abs(trade.NumberShares);
                    }

                    DailyValuation sharesPreviousValue = Shares.ValueBefore(trade.Day) ?? new DailyValuation(DateTime.Today, 0);
                    bool hasShareValue = Shares.TryGetValue(trade.Day, out decimal shareValue);

                    decimal expectedNumberShares = sharesPreviousValue.Value + sign * trade.NumberShares;
                    if ((hasShareValue && !Equals(shareValue, expectedNumberShares)) || !hasShareValue)
                    {
                        Shares.SetData(trade.Day, expectedNumberShares);
                    }
                }

                if (trade.TradeType.IsInvestmentTradeType())
                {
                    Investments.SetData(trade.Day, sign * trade.TotalCost, reportLogger);
                }
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
                            decimal numShares = sharesCurrentValue.Value - sharesPreviousValue.Value;
                            decimal unitPrice = UnitPrice.ValueOnOrBefore(investmentValue.Day).Value;
                            decimal value = numShares * unitPrice;
                            Investments.SetData(investmentValue.Day, value, reportLogger);
                            TradeType trade = value > 0 ? TradeType.Buy : TradeType.Sell;
                            SecurityTrades.Add(new SecurityTrade(trade, Names, investmentValue.Day, Math.Abs(numShares), unitPrice, 0.0m));
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

            decimal previousNumShares = 0.0m;
            for (int index = 0; index < Shares.Count(); index++)
            {
                DailyValuation shareValue = Shares[index];
                Shares.SetData(shareValue.Day, shareValue.Value);
                decimal shareDiff = shareValue.Value - previousNumShares;
                if (!shareDiff.Equals(0.0m))
                {
                    if (!SecurityTrades.Any(trade => trade.Day == shareValue.Day))
                    {
                        SecurityTrades.Add(new SecurityTrade(TradeType.ShareReprice, Names, shareValue.Day, shareDiff, UnitPrice.ValueOnOrBefore(shareValue.Day).Value, 0.0m));
                    }
                }
                previousNumShares = shareValue.Value;
            }

            SetupEventListening();
        }
    }
}
