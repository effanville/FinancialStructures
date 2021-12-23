using System;
using System.Collections.Generic;
using System.Linq;
using Common.Structure.DataStructures;
using Common.Structure.NamingStructures;
using FinancialStructures.DataStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.FinanceStructures.Implementation
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
            SecurityTrade trade = SecurityTrades.Where(t => t.Day.Equals(day)).FirstOrDefault();
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
            List<DailyValuation> values = Investments.GetValuesBetween(Investments.FirstDate(), Investments.LatestDate());
            List<Labelled<TwoName, DailyValuation>> namedValues = new List<Labelled<TwoName, DailyValuation>>();

            foreach (DailyValuation value in values)
            {
                if (value != null && value.Value != 0)
                {
                    value.Value *= GetCurrencyValue(value.Day, currency);
                    namedValues.Add(new Labelled<TwoName, DailyValuation>(new TwoName(Names.Company, Names.Name), value));
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

        /// <inheritdoc/>
        public IReadOnlyList<SecurityDayData> GetDataForDisplay()
        {
            List<SecurityDayData> output = new List<SecurityDayData>();
            if (UnitPrice.Any())
            {
                foreach (DailyValuation unitPriceValuation in UnitPrice.GetValuesBetween(UnitPrice.FirstDate(), UnitPrice.LatestDate()))
                {
                    decimal shares = Shares.ValueOnOrBefore(unitPriceValuation.Day)?.Value ?? 0.0m;
                    _ = Investments.TryGetValue(unitPriceValuation.Day, out decimal invest);
                    SecurityDayData thisday = new SecurityDayData(unitPriceValuation.Day, unitPriceValuation.Value, shares, invest);
                    output.Add(thisday);
                }
            }
            if (Shares.Any())
            {
                foreach (DailyValuation sharesValuation in Shares.GetValuesBetween(Shares.FirstDate(), Shares.LatestDate()))
                {
                    if (!UnitPrice.TryGetValue(sharesValuation.Day, out decimal _))
                    {
                        _ = Investments.TryGetValue(sharesValuation.Day, out decimal invest);
                        decimal unitPriceInterpolation = UnitPrice.Value(sharesValuation.Day)?.Value ?? 0.0m;
                        SecurityDayData thisday = new SecurityDayData(sharesValuation.Day, unitPriceInterpolation, sharesValuation.Value, invest);
                        output.Add(thisday);
                    }
                }
            }
            if (Investments.Any())
            {
                foreach (DailyValuation investmentValuation in Investments.GetValuesBetween(Investments.FirstDate(), Investments.LatestDate()))
                {
                    if (!UnitPrice.TryGetValue(investmentValuation.Day, out decimal _) && !Shares.TryGetValue(investmentValuation.Day, out decimal _))
                    {
                        decimal shares = Shares.ValueOnOrBefore(investmentValuation.Day).Value;
                        decimal unitPriceInterpolation = UnitPrice.Value(investmentValuation.Day)?.Value ?? 0.0m;
                        SecurityDayData thisday = new SecurityDayData(investmentValuation.Day, unitPriceInterpolation, shares, investmentValuation.Value);

                        output.Add(thisday);
                    }
                }
            }
            output.Sort();
            return output;
        }

        private static decimal GetCurrencyValue(DateTime date, ICurrency currency)
        {
            return currency == null ? 1.0m : currency.Value(date)?.Value ?? 1.0m;
        }
    }
}
