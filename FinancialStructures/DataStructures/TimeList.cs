using System.Collections.Generic;
using System.Linq;

namespace FinancialStructures.DataStructures
{
    /// <summary>
    /// Sorted list of values, with last value the most recent, and first the oldest.
    /// </summary>
    /// <remarks>This list is sorted, with oldest value the first and latest the last.</remarks>
    public partial class TimeList
    {

        /// <summary>
        /// Collection of data within the TimeList.
        /// </summary>
        private List<DailyValuation> fValues;

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        public List<DailyValuation> Values
        {
            get { return fValues; }
            set { fValues = value; }
        }

        /// <summary>
        /// Obtains a copy of the data.
        /// </summary>
        public DailyValuation this[int index]
        {
            get { return new DailyValuation(fValues[index]); }
        }

        /// <summary>
        /// Constructor adding values.
        /// </summary>
        /// <remarks>For testing only.</remarks>
        public TimeList(List<DailyValuation> values)
        {
            fValues = values;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TimeList()
        {
            fValues = new List<DailyValuation>();
        }

        /// <summary>
        /// Returns true if contains any entries. 
        /// </summary>
        internal bool Any()
        {
            return fValues != null && fValues.Any();
        }

        /// <summary>
        /// Returns the number of valuations in the timelist.
        /// </summary>
        public int Count()
        {
            return fValues.Count;
        }
    }
}
