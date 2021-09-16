using System;
using Common.Structure.FinanceFunctions;

namespace FinancialStructures.FinanceStructures.Implementation
{
    public partial class ValueList
    {
        /// <summary>
        /// returns compound annual rate of security between the two times specified
        /// </summary>
        public virtual double CAR(DateTime earlierTime, DateTime laterTime)
        {
            return FinancialFunctions.CAR(Value(earlierTime), Value(laterTime));
        }
    }
}
