using System.Collections.Generic;
using StructureCommon.DataStructures;

namespace FinancialStructures.FinanceStructures.Implementation
{
    public partial class ValueList
    {
        /// <summary>
        /// Retrieves data in a list ordered by date.
        /// </summary>
        public List<DailyValuation> GetDataForDisplay()
        {
            List<DailyValuation> output = new List<DailyValuation>();
            if (Values.Any())
            {
                foreach (DailyValuation datevalue in Values.GetValuesBetween(Values.FirstDate(), Values.LatestDate()))
                {
                    _ = Values.TryGetValue(datevalue.Day, out double UnitPrice);
                    DailyValuation thisday = new DailyValuation(datevalue.Day, UnitPrice);
                    output.Add(thisday);
                }
            }

            return output;
        }
    }
}
