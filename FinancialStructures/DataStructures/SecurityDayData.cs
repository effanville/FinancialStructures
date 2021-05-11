using System;
using FinancialStructures.FinanceStructures;
using StructureCommon.Extensions;

namespace FinancialStructures.DataStructures
{
    /// <summary>
    /// Contains a single day record of the data in a <see cref="ISecurity"/>.
    /// </summary>
    public class SecurityDayData : IComparable
    {
        /// <summary>
        /// The date of this valuation.
        /// </summary>
        public DateTime Date
        {
            get;
            set;
        }

        /// <summary>
        /// The unit price of on this day.
        /// </summary>
        public double UnitPrice
        {
            get;
            set;
        }

        /// <summary>
        /// The number of shares held on this day.
        /// </summary>
        public double ShareNo
        {
            get;
            set;
        }

        /// <summary>
        /// The total value of this security on this day.
        /// </summary>
        public double Value
        {
            get
            {
                return UnitPrice * ShareNo;
            }
        }

        /// <summary>
        /// The value of an investment made on this day.
        /// </summary>
        public double NewInvestment
        {
            get;
            set;
        }

        /// <summary>
        /// Create an empty <see cref="SecurityDayData"/>
        /// </summary>
        public SecurityDayData()
        {
            Date = DateTime.Today;
        }

        /// <summary>
        /// Create a <see cref="SecurityDayData"/> from the specified values.
        /// </summary>
        public SecurityDayData(DateTime date, double unitPrice, double shareNo, double newInvestment)
        {
            Date = date;
            UnitPrice = unitPrice;
            ShareNo = shareNo;
            NewInvestment = newInvestment;
        }

        /// <summary>
        /// Create a copy of this <see cref="SecurityDayData"/>
        /// </summary>
        public SecurityDayData Copy()
        {
            return new SecurityDayData(Date, UnitPrice, ShareNo, NewInvestment);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Date.ToUkDateStringPadded()}, {UnitPrice}, {ShareNo}, {NewInvestment}";
        }

        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            if (obj is SecurityDayData dailyView)
            {
                return DateTime.Compare(Date, dailyView.Date);
            }
            return 0;
        }
    }
}
