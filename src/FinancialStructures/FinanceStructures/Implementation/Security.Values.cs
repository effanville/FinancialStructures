using System;
using System.Collections.Generic;
using System.Linq;

using Effanville.Common.Structure.DataStructures;
using Effanville.Common.Structure.NamingStructures;
using Effanville.FinancialStructures.DataStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.FinanceStructures.Implementation
{
    public partial class Security
    {
        /// <summary>
        /// Returns data for the specific day.
        /// </summary>
        public SecurityDayData DayData(DateTime day)
        {
            _ = UnitPrice.TryGetValue(day, out decimal unitPrice);
            _ = Shares.TryGetValue(day, out decimal shares);
            _ = Investments.TryGetValue(day, out decimal invest);
            SecurityTrade trade = Trades.FirstOrDefault(t => t.Day.Equals(day));
            return new SecurityDayData(day, unitPrice, shares, invest, trade);
        }

        /// <inheritdoc/>
        public DailyValuation LastInvestment(ICurrency currency)
        {
            if (Investments.Any())
            {
                return Investments.Values().Last();
            }

            return null;
        }

        /// <inheritdoc/>
        public List<Labelled<TwoName, DailyValuation>> AllInvestmentsNamed(ICurrency currency = null)
        {
            List<DailyValuation> values =
                Investments.GetValuesBetween(Investments.FirstDate(), Investments.LatestDate());
            List<Labelled<TwoName, DailyValuation>> namedValues = new List<Labelled<TwoName, DailyValuation>>();

            foreach (DailyValuation value in values)
            {
                if (value != null && value.Value != 0)
                {
                    value.Value *= GetCurrencyValue(value.Day, currency);
                    namedValues.Add(
                        new Labelled<TwoName, DailyValuation>(new TwoName(Names.Company, Names.Name), value));
                }
            }

            return namedValues;
        }

        /// <summary>
        /// Retrieves data in a list ordered by date.
        /// </summary>
        public override List<DailyValuation> ListOfValues()
        {
            IReadOnlyList<SecurityDayData> output = GetDataForDisplay();
            List<DailyValuation> thing = new List<DailyValuation>();
            foreach (SecurityDayData dateValue in output)
            {
                thing.Add(new DailyValuation(dateValue.Date, dateValue.Value));
            }

            return thing;
        }

        private List<SecurityDayData> _displayData;

        SecurityDayData Calc(
            DateTime time,
            List<DailyValuation> unitPrices,
            List<DailyValuation> shs,
            List<DailyValuation> investments,
            int unitPriceIndex,
            int shsIndex,
            int investmentIndex)
        {
            decimal unitPriceValue;
            if (unitPrices.Any())
            {
                if (unitPriceIndex == 0)
                {
                    unitPriceValue = unitPrices[unitPriceIndex].Value;
                }
                else if (unitPriceIndex == unitPrices.Count)
                {
                    unitPriceValue = unitPrices[unitPriceIndex - 1].Value;
                }
                else if (time == unitPrices[unitPriceIndex].Day)
                {
                    unitPriceValue = unitPrices[unitPriceIndex].Value;
                }
                else
                {
                    var currentValuation = unitPrices[unitPriceIndex];
                    DailyValuation earlier =
                        time < currentValuation.Day ? unitPrices[unitPriceIndex - 1] : currentValuation;
                    DailyValuation later = time < currentValuation.Day
                        ? currentValuation
                        : unitPrices[unitPriceIndex + 1];
                    unitPriceValue = earlier.Value + (later.Value - earlier.Value) /
                        (decimal)(later.Day - earlier.Day).TotalDays * (decimal)(time - earlier.Day).TotalDays;
                }
            }
            else { unitPriceValue = 0; }

            decimal numShares;
            if (shsIndex < shs.Count && time == shs[shsIndex].Day)
            {
                numShares = shs[shsIndex].Value;
            }
            else 
            { 
                numShares = shsIndex == 0
                    ? 0.0m
                    : shs[shsIndex - 1].Value;
            }

            decimal investmentValue = investmentIndex < investments.Count && time == investments[investmentIndex].Day
                ? investments[investmentIndex].Value
                : 0.0m;

            return new SecurityDayData(time, unitPriceValue, numShares, investmentValue);
        }

        DateTime GetEarliestTime(
            List<DailyValuation> unitPrices,
            List<DailyValuation> shs,
            List<DailyValuation> investments,
            int unitPriceIndex,
            int shsIndex,
            int investmentIndex)
        {
            DateTime currentTime =
                (unitPriceIndex < unitPrices.Count && shsIndex < shs.Count && unitPrices[unitPriceIndex].Day < shs[shsIndex].Day)
                || unitPriceIndex < unitPrices.Count
                    ? unitPrices[unitPriceIndex].Day
                    : shsIndex < shs.Count
                        ? shs[shsIndex].Day
                        : default;

            if (investmentIndex < investments.Count && currentTime > investments[investmentIndex].Day)
            {
                currentTime = investments[investmentIndex].Day;
            }

            return currentTime;
        }

        DateTime NextTime(
            DateTime currentTime,
            List<DailyValuation> unitPrices,
            List<DailyValuation> shs,
            List<DailyValuation> investments,
            ref int unitPriceIndex,
            ref int shsIndex,
            ref int investmentIndex)
        {
            if (unitPriceIndex < unitPrices.Count && unitPrices[unitPriceIndex].Day == currentTime)
            {
                unitPriceIndex++;
            }

            if (shsIndex < shs.Count && shs[shsIndex].Day == currentTime)
            {
                shsIndex++;
            }

            if (investmentIndex < investments.Count && investments[investmentIndex].Day == currentTime)
            {
                investmentIndex++;
            }

            return GetEarliestTime(unitPrices, shs, investments, unitPriceIndex, shsIndex, investmentIndex);
        }

        /// <inheritdoc/>
        public IReadOnlyList<SecurityDayData> GetDataForDisplay()
        {
            if (_displayData != null)
            {
                return _displayData;
            }

            List<DailyValuation> unitPrices = UnitPrice.Values();
            var shs = Shares.Values();
            var investments = Investments.Values();
            int unitPriceIndex = 0;
            int shsIndex = 0;
            int investmentIndex = 0;
            List<SecurityDayData> output = new List<SecurityDayData>();
            DateTime currentTime =
                GetEarliestTime(unitPrices, shs, investments, unitPriceIndex, shsIndex, investmentIndex);
            while (unitPriceIndex < unitPrices.Count || shsIndex < shs.Count || investmentIndex < investments.Count)
            {
                output.Add(Calc(currentTime, unitPrices, shs, investments, unitPriceIndex, shsIndex, investmentIndex));
                currentTime = NextTime(currentTime, unitPrices, shs, investments, ref unitPriceIndex, ref shsIndex,
                    ref investmentIndex);
            }

            _displayData = output;
            return output;
        }

        private static decimal GetCurrencyValue(DateTime date, ICurrency currency)
        {
            return currency == null ? 1.0m : currency.Value(date)?.Value ?? 1.0m;
        }
    }
}