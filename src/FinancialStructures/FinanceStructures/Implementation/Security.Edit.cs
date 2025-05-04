using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Effanville.Common.Structure.DataEdit;
using Effanville.Common.Structure.DataStructures;
using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.DataStructures;

namespace Effanville.FinancialStructures.FinanceStructures.Implementation
{
    /// <summary>
    /// Contains data editing of a security class.
    /// </summary>
    public partial class Security
    {
        /// <inheritdoc/>
        public override UpdateResult<DailyValuation> TryEditData(DateTime oldDate, DateTime newDate, decimal value)
        {
            UpdateResult<DailyValuation> result = AddOrEditData(UnitPrice, oldDate, newDate, value);
            if (result.Success)
            {
                EnsureDataConsistency();
            }

            return result;
        }

        /// <inheritdoc/>
        public override UpdateResult<DailyValuation> SetData(DateTime date, decimal value)
        {
            UpdateResult<DailyValuation> result = AddOrEditData(UnitPrice, date, date, value);
            if (result.Success)
            {
                EnsureDataConsistency();
            }

            return result;
        }

        internal bool AddOrEditData(DateTime oldDate, DateTime newDate, decimal unitPrice, decimal shares, decimal investment = 0, SecurityTrade trade = null)
        {
            bool editUnitPrice = AddOrEditData(UnitPrice, oldDate, newDate, unitPrice).Success;
            bool editShares = AddOrEditData(Shares, oldDate, newDate, shares).Success;
            bool editInvestments = AddOrEditData(Investments, oldDate, newDate, investment).Success;
            if (trade != null)
            {
                AddOrEditTrade(oldDate, trade);
            }

            if (editUnitPrice | editShares | editInvestments)
            {
                EnsureDataConsistency();
            }

            return editUnitPrice & editShares & editInvestments;
        }

        /// <inheritdoc/>
        public UpdateResult<SecurityTrade> TryAddOrEditTradeData(SecurityTrade oldTrade, SecurityTrade newTrade)
        {
            UpdateResult<SecurityTrade> result = AddOrEditTrade(oldTrade.Day, newTrade);
            if (result.Success)
            {
                EnsureDataConsistency();
            }

            return result;
        }

        private UpdateResult<SecurityTrade> AddOrEditTrade(DateTime oldDate, SecurityTrade trade)
        {
            try
            {
                lock (TradesLock)
                {
                    if (!SecurityTrades.Any(existingTrade => existingTrade.Day.Equals(oldDate)))
                    {
                        SecurityTrades.Add(trade);
                        return UpdateResult.Add(trade.Copy());
                    }

                    foreach (SecurityTrade tradeVal in SecurityTrades)
                    {
                        if (tradeVal.Day.Equals(oldDate))
                        {
                            SecurityTrade oldValue = tradeVal.Copy();
                            tradeVal.Day = trade.Day;
                            tradeVal.NumberShares = trade.NumberShares;
                            tradeVal.UnitPrice = trade.UnitPrice;
                            tradeVal.TradeCosts = trade.TradeCosts;
                            tradeVal.TradeType = trade.TradeType;

                            return UpdateResult.Change(oldValue, tradeVal.Copy());
                        }
                    }

                    return UpdateResult.Fail(trade, isAdd: true, isChange: true);
                }
            }
            finally
            {
                SecurityTrades.Sort();
            }
        }

        private static UpdateResult<DailyValuation> AddOrEditData(TimeList list, DateTime oldDate, DateTime date, decimal value)
        {
            if (list.ValueExists(oldDate, out _))
            {
                return list.TryEditData(oldDate, date, value);
            }

            return list.SetData(date, value);
        }

        /// <inheritdoc/>
        public override List<object> CreateDataFromCsv(List<string[]> valuationsToRead, IReportLogger reportLogger = null)
        {
            List<object> dailyValuations = new List<object>();
            foreach (string[] dayValuation in valuationsToRead)
            {
                if (dayValuation.Length != 4)
                {
                    reportLogger?.Error(nameof(Security), "Line in Csv file has incomplete data.");
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
        public override void WriteDataToCsv(TextWriter writer)
        {
            foreach (SecurityDayData value in GetDataForDisplay())
            {
                writer.WriteLine(value.ToString());
            }
        }

        /// <summary>
        /// Tries to delete the data. If it can, it deletes all data specified, then returns true only if all data has been successfully deleted.
        /// </summary>
        public override UpdateResult<DailyValuation> TryDeleteData(DateTime date)
        {
            UpdateResult<DailyValuation> unitDel = UnitPrice.TryDeleteValue(date);
            UpdateResult<DailyValuation> sharesDel = Shares.TryDeleteValue(date);
            UpdateResult<DailyValuation> invDel = Investments.TryDeleteValue(date);
            EnsureDataConsistency();
            return UpdateResult.All(new[] { unitDel, sharesDel, invDel });
        }

        /// <inheritdoc/>
        public UpdateResult<SecurityTrade> TryDeleteTradeData(DateTime date)
        {
            try
            {
                lock (TradesLock)
                {
                    for (int i = 0; i < SecurityTrades.Count; i++)
                    {
                        if (SecurityTrades[i].Day == date)
                        {
                            SecurityTrade value = SecurityTrades[i].Copy();
                            SecurityTrades.RemoveAt(i);
                            return UpdateResult.Delete(value);
                        }
                    }

                    return UpdateResult.Fail(new SecurityTrade(date), isDelete: true);
                }
            }
            finally
            {
                EnsureDataConsistency();
            }
        }

        /// <inheritdoc/>
        public void CleanData()
        {
            Shares.CleanValues();
            Investments.CleanValues(0.0);
        }

        /// <inheritdoc/>
        public void MigrateRepriceToReset()
        {
            var trades = SecurityTrades;
            for (int index = 0; index < trades.Count; index++)
            {
                var trade = trades[index];
                if (trade.TradeType == TradeType.ShareReprice)
                {
                    DailyValuation sharesPreviousValue = Shares.ValueBefore(trade.Day) ?? new DailyValuation(trade.Day, 0);
                    trade.TradeType = TradeType.ShareReset;
                    trade.NumberShares = sharesPreviousValue.Value + trade.NumberShares;
                }

                EnsureDataConsistency();
            }
        }

        /// <summary>
        /// Upon a new/edit/Delete trade, one needs to recompute the values of the investments for that trade.
        /// </summary>
        internal void EnsureDataConsistency()
        {
            CleanData();

            var trades = Trades.ToList();
            // When a trade is present, number of shares bought/sold should correspond to share number difference before
            // and after.
            // Investment on that day should correspond also.

            // First remove all share values that dont have a trade value.
            // Do this first to ensure that share totals when editing trades are correct.
            for (int index = 0; index < Shares.Count(); index++)
            {
                DailyValuation shareValue = Shares[index];
                if (!trades.Any(trade => trade.Day.Equals(shareValue.Day)))
                {
                    if (Shares.TryDeleteValue(shareValue.Day).Success)
                    {
                        index--;
                    }
                }
            }

            // Cycle through all trades as trade can impact later share numbers.
            // Requires trades to be sorted in date order. (this should be a no-op)
            trades.Sort();
            foreach (SecurityTrade trade in trades)
            {
                if (trade != null)
                {
                    // if trade should alter number of shares, then alter
                    // if it shouldnt then remove the shares.
                    if (trade.TradeType.IsShareNumberAlteringTradeType())
                    {
                        DailyValuation sharesPreviousValue = Shares.ValueBefore(trade.Day) ?? new DailyValuation(trade.Day, 0);
                        bool hasShareValue = Shares.TryGetValue(trade.Day, out decimal shareValue);
                        decimal expectedNumberShares = trade.GetPostTradeShares(sharesPreviousValue.Value);
                        if ((hasShareValue && !Equals(shareValue, expectedNumberShares)) || !hasShareValue)
                        {
                            Shares.SetData(trade.Day, expectedNumberShares);
                        }
                    }
                    else
                    {
                        _ = Shares.TryDeleteValue(trade.Day);
                    }

                    // if trade should have investment value, then set the value, if it
                    // shouldnt have value then remove.
                    if (trade.TradeType.IsInvestmentTradeType())
                    {
                        decimal sign = trade.TradeType.Sign();
                        Investments.SetData(trade.Day, sign * trade.TotalCost);
                    }
                    else
                    {
                        _ = Investments.TryDeleteValue(trade.Day);
                    }
                }
            }

            // now cycle through Investments removing values that no longer have trades.
            for (int index = 0; index < Investments.Count(); index++)
            {
                DailyValuation investmentValue = Investments[index];
                if (!trades.Any(trade => trade.Day.Equals(investmentValue.Day)))
                {
                    if (Investments.TryDeleteValue(investmentValue.Day).Success)
                    {
                        index--;
                    }
                }
            }
        }

        /// <summary>
        /// Upon a load of security from file, one needs to recompute the values of the investments
        /// One should not change Inv = 0 or Inv > 0  to ensure that dividend reivestments are not accidentally included in a new investment.
        /// This though causes a problem if a value is deleted.
        /// One adds new trades here if trades do not exist to deal with migrating from an old xml form.
        /// </summary>
        internal void EnsureOnLoadDataConsistency()
        {
            CleanData();
            SecurityTrades.Sort();
            for (int index = 0; index < SecurityTrades.Count; index++)
            {
                // When a trade is present, number of shares bought/sold should correspond to share number difference before
                // and after.
                // Investment on that day should correspond also.
                SecurityTrade trade = SecurityTrades[index];
                if (trade.TradeType.IsShareNumberAlteringTradeType())
                {
                    if (trade.TradeType == TradeType.Sell)
                    {
                        trade.NumberShares = Math.Abs(trade.NumberShares);
                    }

                    DailyValuation sharesPreviousValue = Shares.ValueBefore(trade.Day) ?? new DailyValuation(trade.Day, 0);
                    bool hasShareValue = Shares.TryGetValue(trade.Day, out decimal shareValue);

                    decimal expectedNumberShares = trade.GetPostTradeShares(sharesPreviousValue.Value);
                    if ((hasShareValue && !Equals(shareValue, expectedNumberShares)) || !hasShareValue)
                    {
                        Shares.SetData(trade.Day, expectedNumberShares);
                    }
                }

                if (trade.TradeType.IsInvestmentTradeType())
                {
                    decimal sign = trade.TradeType.Sign();
                    Investments.SetData(trade.Day, sign * trade.TotalCost);
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
                            decimal unitPrice = UnitPrice.ValueOnOrBefore(investmentValue.Day)?.Value ?? 0.0m;
                            decimal value = numShares * unitPrice;
                            Investments.SetData(investmentValue.Day, value);
                            TradeType trade = value > 0 ? TradeType.Buy : TradeType.Sell;
                            SecurityTrades.Add(new SecurityTrade(trade, Names, investmentValue.Day, Math.Abs(numShares), unitPrice, 0.0m));
                        }
                    }
                }
                if (investmentValue.Value == 0)
                {
                    if (Investments.TryDeleteValue(investmentValue.Day).Success)
                    {
                        index--;
                    }
                }
            }

            for (int index = 0; index < Shares.Count(); index++)
            {
                DailyValuation shareValue = Shares[index];
                decimal numberShares = shareValue.Value;
                Shares.SetData(shareValue.Day, numberShares);
                if (!SecurityTrades.Any(trade => trade.Day == shareValue.Day))
                {
                    SecurityTrades.Add(new SecurityTrade(TradeType.ShareReset, Names, shareValue.Day, numberShares, UnitPrice.ValueOnOrBefore(shareValue.Day).Value, 0.0m));
                }
            }
        }
    }
}
