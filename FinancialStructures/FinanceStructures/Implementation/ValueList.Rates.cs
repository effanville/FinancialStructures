using System;
using Common.Structure.MathLibrary.Finance;

namespace FinancialStructures.FinanceStructures.Implementation
{
    public partial class ValueList
    {
        /// <summary>
        /// returns compound annual rate of security between the two times specified
        /// </summary>
        public virtual double CAR(DateTime earlierTime, DateTime laterTime)
        {
            return FinanceFunctions.CAR(Value(earlierTime), Value(laterTime));
        }
    }
}
