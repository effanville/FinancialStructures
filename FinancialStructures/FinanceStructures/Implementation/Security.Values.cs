using System;
using System.Collections.Generic;
using FinancialStructures.DataStructures;
using Common.Structure.DataStructures;

namespace FinancialStructures.FinanceStructures.Implementation
{
    public partial class Security
    {
        /// <summary>
        /// Returns data for the specific day.
        /// </summary>
        public SecurityDayData DayData(DateTime day)
        {
            _ = UnitPrice.TryGetValue(day, out double unitPrice);
            _ = Shares.TryGetValue(day, out double shares);
            _ = Investments.TryGetValue(day, out double invest);
            return new SecurityDayData(day, unitPrice, shares, invest);
        }

        /// <inheritdoc/>
        public List<DayValue_Named> AllInvestmentsNamed(ICurrency currency = null)
        {
            List<DailyValuation> values = Investments.GetValuesBetween(Investments.FirstDate(), Investments.LatestDate());
            List<DayValue_Named> namedValues = new List<DayValue_Named>();

            foreach (DailyValuation value in values)
            {
                if (value != null && value.Value != 0)
                {
                    value.Value = value.Value * GetCurrencyValue(value.Day, currency);
                    namedValues.Add(new DayValue_Named(Names.Company, Names.Name, value));
                }
            }

            return namedValues;
        }

        /// <summary>
        /// Retrieves data in a list ordered by date.
        /// </summary>
        public override List<DailyValuation> ListOfValues()
        {
            var output = GetDataForDisplay();
            List<DailyValuation> thing = new List<DailyValuation>();
            foreach (var dateValue in output)
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
                    double shares = Shares.NearestEarlierValue(unitPriceValuation.Day).Value;
                    _ = Investments.TryGetValue(unitPriceValuation.Day, out double invest);
                    SecurityDayData thisday = new SecurityDayData(unitPriceValuation.Day, unitPriceValuation.Value, shares, invest);
                    output.Add(thisday);
                }
            }
            if (Shares.Any())
            {
                foreach (DailyValuation sharesValuation in Shares.GetValuesBetween(Shares.FirstDate(), Shares.LatestDate()))
                {
                    if (!UnitPrice.TryGetValue(sharesValuation.Day, out double _))
                    {
                        _ = Investments.TryGetValue(sharesValuation.Day, out double invest);
                        SecurityDayData thisday = new SecurityDayData(sharesValuation.Day, double.NaN, sharesValuation.Value, invest);
                        output.Add(thisday);
                    }
                }
            }
            if (Investments.Any())
            {
                foreach (DailyValuation investmentValuation in Investments.GetValuesBetween(Investments.FirstDate(), Investments.LatestDate()))
                {
                    if (!UnitPrice.TryGetValue(investmentValuation.Day, out double _) && !Shares.TryGetValue(investmentValuation.Day, out double _))
                    {
                        SecurityDayData thisday = new SecurityDayData(investmentValuation.Day, double.NaN, double.NaN, investmentValuation.Value);

                        output.Add(thisday);
                    }
                }
            }
            output.Sort();
            return output;
        }

        private double GetCurrencyValue(DateTime date, ICurrency currency)
        {
            return currency == null ? 1.0 : currency.Value(date)?.Value ?? 1.0;
        }

        /// <summary>
        /// Checks if SharePrice data for the date specified exists. if so outputs index value
        /// </summary>
        private bool DoesDateSharesDataExist(DateTime date, out int index)
        {
            return Shares.ValueExists(date, out index);
        }

        /// <summary>
        /// Checks if UnitPrice data for the date specified exists. if so outputs index value
        /// </summary>
        private bool DoesDateUnitPriceDataExist(DateTime date, out int index)
        {
            return UnitPrice.ValueExists(date, out index);
        }

        /// <summary>
        /// Checks if UnitPrice data for the date specified exists. if so outputs index value
        /// </summary>
        private bool DoesDateInvestmentDataExist(DateTime date, out int index)
        {
            return Investments.ValueExists(date, out index);
        }
    }
}
