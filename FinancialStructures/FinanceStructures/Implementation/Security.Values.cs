using System;
using System.Collections.Generic;
using FinancialStructures.DataStructures;
using StructureCommon.DataStructures;

namespace FinancialStructures.FinanceStructures.Implementation
{
    public partial class Security
    {
        /// <summary>
        /// Checks if SharePrice data for the date specified exists. if so outputs index value
        /// </summary>
        internal bool DoesDateSharesDataExist(DateTime date, out int index)
        {
            return fShares.ValueExists(date, out index);
        }

        /// <summary>
        /// Checks if UnitPrice data for the date specified exists. if so outputs index value
        /// </summary>
        internal bool DoesDateUnitPriceDataExist(DateTime date, out int index)
        {
            return fUnitPrice.ValueExists(date, out index);
        }

        /// <summary>
        /// Checks if UnitPrice data for the date specified exists. if so outputs index value
        /// </summary>
        internal bool DoesDateInvestmentDataExist(DateTime date, out int index)
        {
            return fInvestments.ValueExists(date, out index);
        }

        /// <inheritdoc/>
        public bool IsEqualTo(IValueList otherList)
        {
            if (otherList is ISecurity otherSecurity)
            {
                return IsEqualTo(otherSecurity);
            }

            return false;
        }

        /// <inheritdoc/>
        public bool IsEqualTo(ISecurity otherSecurity)
        {
            if (otherSecurity.Names.Name != Names.Name)
            {
                return false;
            }

            if (otherSecurity.Names.Company != Names.Company)
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public int Count()
        {
            return fUnitPrice.Count();
        }

        public SecurityDayData DayData(DateTime day)
        {
            fUnitPrice.TryGetValue(day, out double UnitPrice);
            fShares.TryGetValue(day, out double shares);
            fInvestments.TryGetValue(day, out double invest);
            return new SecurityDayData(day, UnitPrice, shares, invest);
        }

        /// <summary>
        /// Produces a list of data for visual display purposes. Display in the base currency of the fund ( so this does not modify values due to currency)
        /// </summary>
        public List<SecurityDayData> GetDataForDisplay()
        {
            List<SecurityDayData> output = new List<SecurityDayData>();
            if (fUnitPrice.Any())
            {
                foreach (DailyValuation datevalue in fUnitPrice.GetValuesBetween(fUnitPrice.FirstDate(), fUnitPrice.LatestDate()))
                {
                    fUnitPrice.TryGetValue(datevalue.Day, out double UnitPrice);
                    fShares.TryGetValue(datevalue.Day, out double shares);
                    fInvestments.TryGetValue(datevalue.Day, out double invest);
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
                        fShares.TryGetValue(datevalue.Day, out double shares);
                        fInvestments.TryGetValue(datevalue.Day, out double invest);
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
                        fInvestments.TryGetValue(datevalue.Day, out double invest);
                        SecurityDayData thisday = new SecurityDayData(datevalue.Day, double.NaN, double.NaN, invest);

                        output.Add(thisday);
                    }
                }
            }
            output.Sort();
            return output;
        }
    }
}
