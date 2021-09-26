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
        public virtual DailyValuation RecentPreviousValue(DateTime date)
        {
            DailyValuation val = Values.RecentPreviousValue(date);
            if (val == null)
            {
                return new DailyValuation(date, 0.0);
            }

            return val;
        }

        /// <inheritdoc/>
        public virtual DailyValuation NearestEarlierValuation(DateTime date)
        {
            return Values.NearestEarlierValue(date);
        }

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
