using System;
using System.Collections.Generic;
using FinancialStructures.DataStructures;
using StructureCommon.DataStructures;

namespace FinancialStructures.FinanceStructures.Implementation
{
    public partial class Security
    {
        /// <summary>
        /// Returns data for the specific day.
        /// </summary>
        public SecurityDayData DayData(DateTime day)
        {
            _ = fUnitPrice.TryGetValue(day, out double UnitPrice);
            _ = fShares.TryGetValue(day, out double shares);
            _ = fInvestments.TryGetValue(day, out double invest);
            return new SecurityDayData(day, UnitPrice, shares, invest);
        }

        /// <inheritdoc/>
        public List<DayValue_Named> AllInvestmentsNamed(ICurrency currency = null)
        {
            List<DailyValuation> values = fInvestments.GetValuesBetween(fInvestments.FirstDate(), fInvestments.LatestDate());
            List<DayValue_Named> namedValues = new List<DayValue_Named>();

            foreach (DailyValuation value in values)
            {
                if (value != null && value.Value != 0)
                {
                    double currencyValue = currency == null ? 1.0 : currency.Value(value.Day).Value;
                    value.SetValue(value.Value * currencyValue);
                    namedValues.Add(new DayValue_Named(Names.Company, Names.Name, value));
                }
            }

            return namedValues;
        }

        /// <inheritdoc/>
        public new List<SecurityDayData> GetDataForDisplay()
        {
            List<SecurityDayData> output = new List<SecurityDayData>();
            if (fUnitPrice.Any())
            {
                foreach (DailyValuation datevalue in fUnitPrice.GetValuesBetween(fUnitPrice.FirstDate(), fUnitPrice.LatestDate()))
                {
                    _ = fUnitPrice.TryGetValue(datevalue.Day, out double UnitPrice);
                    _ = fShares.TryGetValue(datevalue.Day, out double shares);
                    _ = fInvestments.TryGetValue(datevalue.Day, out double invest);
                    SecurityDayData thisday = new SecurityDayData(datevalue.Day, UnitPrice, shares, invest);
                    output.Add(thisday);
                }
            }
            if (fShares.Any())
            {
                foreach (DailyValuation datevalue in fShares.GetValuesBetween(fShares.FirstDate(), fShares.LatestDate()))
                {
                    if (!fUnitPrice.TryGetValue(datevalue.Day, out double _))
                    {
                        _ = fShares.TryGetValue(datevalue.Day, out double shares);
                        _ = fInvestments.TryGetValue(datevalue.Day, out double invest);
                        SecurityDayData thisday = new SecurityDayData(datevalue.Day, double.NaN, shares, invest);
                        output.Add(thisday);
                    }
                }
            }
            if (fInvestments.Any())
            {
                foreach (DailyValuation datevalue in fInvestments.GetValuesBetween(fInvestments.FirstDate(), fInvestments.LatestDate()))
                {
                    if (!fUnitPrice.TryGetValue(datevalue.Day, out double _) && !fShares.TryGetValue(datevalue.Day, out double _))
                    {
                        _ = fInvestments.TryGetValue(datevalue.Day, out double invest);
                        SecurityDayData thisday = new SecurityDayData(datevalue.Day, double.NaN, double.NaN, invest);

                        output.Add(thisday);
                    }
                }
            }
            output.Sort();
            return output;
        }

        /// <summary>
        /// Checks if SharePrice data for the date specified exists. if so outputs index value
        /// </summary>
        private bool DoesDateSharesDataExist(DateTime date, out int index)
        {
            return fShares.ValueExists(date, out index);
        }

        /// <summary>
        /// Checks if UnitPrice data for the date specified exists. if so outputs index value
        /// </summary>
        private bool DoesDateUnitPriceDataExist(DateTime date, out int index)
        {
            return fUnitPrice.ValueExists(date, out index);
        }

        /// <summary>
        /// Checks if UnitPrice data for the date specified exists. if so outputs index value
        /// </summary>
        private bool DoesDateInvestmentDataExist(DateTime date, out int index)
        {
            return fInvestments.ValueExists(date, out index);
        }
    }
}
