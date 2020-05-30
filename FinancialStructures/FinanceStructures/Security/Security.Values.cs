using System;
using System.Collections.Generic;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;

namespace FinancialStructures.FinanceStructures
{
    public partial class Security
    {
        /// <summary>
        /// Makes a copy of the security.
        /// </summary>
        public ISecurity Copy()
        {
            return new Security(Names, fShares, fUnitPrice, fInvestments);
        }

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
        /// <summary>
        /// Compares another security and determines if has same name and company.
        /// </summary>
        public bool IsEqualTo(ISecurity otherSecurity)
        {
            if (otherSecurity.Name != Names.Name)
            {
                return false;
            }

            if (otherSecurity.Company != Names.Company)
            {
                return false;
            }

            return true;
        }

        public bool SameName(TwoName otherNames)
        {
            return Names.IsEqualTo(otherNames);
        }

        public bool SameName(string company, string name)
        {
            if (name != Names.Name)
            {
                return false;
            }

            if (company != Names.Company)
            {
                return false;
            }

            return true;
        }

        public int Count()
        {
            return fUnitPrice.Count();
        }
        /// <summary>
        /// Returns true if shares and unit prices have an item or are not null.
        /// </summary>
        public bool Any()
        {
            if (fUnitPrice.Any() && fShares.Any())
            {
                return true;
            }

            return false;
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
