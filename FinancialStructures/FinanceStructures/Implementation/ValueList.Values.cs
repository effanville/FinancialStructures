using System.Collections.Generic;
using Common.Structure.DataStructures;

namespace FinancialStructures.FinanceStructures.Implementation
{
    public partial class ValueList
    {
        /// <summary>
        /// Retrieves data in a list ordered by date.
        /// </summary>
        public virtual List<DailyValuation> ListOfValues()
        {
            List<DailyValuation> output = new List<DailyValuation>();
            if (Values.Any())
            {
                foreach (DailyValuation dateValue in Values.Values())
                {
                    output.Add(dateValue.Copy());
                }
            }

            return output;
        }
    }
}
