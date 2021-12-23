using System;
using System.Collections.Generic;
using Common.Structure.DataStructures;

namespace FinancialStructures.FinanceStructures.Implementation
{
    public partial class ValueList
    {
        /// <inheritdoc/>
        public virtual DailyValuation LatestValue()
        {
            return Values.LatestValuation();
        }

        /// <inheritdoc/>
        public virtual DailyValuation FirstValue()
        {
            return Values.FirstValuation();
        }

        /// <inheritdoc/>
        public virtual DailyValuation Value(DateTime date)
        {
            return Values.Value(date);
        }

        /// <inheritdoc/>
        public virtual DailyValuation ValueBefore(DateTime date)
        {
            DailyValuation val = Values.ValueBefore(date);
            if (val == null)
            {
                return new DailyValuation(date, 0.0m);
            }

            return val;
        }

        /// <inheritdoc/>
        public virtual DailyValuation ValueOnOrBefore(DateTime date)
        {
            return Values.ValueOnOrBefore(date);
        }

        /// <summary>
        /// Retrieves a copy of all the data in a list ordered by date.
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
