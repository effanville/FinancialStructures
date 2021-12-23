using System;
using Common.Structure.Extensions;
using FinancialStructures.FinanceStructures;

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
        public decimal UnitPrice
        {
            get;
            set;
        }

        /// <summary>
        /// The number of shares held on this day.
        /// </summary>
        public decimal ShareNo
        {
            get;
            set;
        }

        /// <summary>
        /// The total value of this security on this day.
        /// </summary>
        public decimal Value => UnitPrice * ShareNo;

        /// <summary>
        /// The value of an investment made on this day.
        /// </summary>
        public decimal NewInvestment
        {
            get;
            set;
        }

        /// <summary>
        /// Any trade that took place on this day.
        /// </summary>
        public SecurityTrade Trade
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
        public SecurityDayData(DateTime date, decimal unitPrice, decimal shareNo, decimal newInvestment, SecurityTrade trade = null)
        {
            Date = date;
            UnitPrice = unitPrice;
            ShareNo = shareNo;
            NewInvestment = newInvestment;
            Trade = trade;
        }

        /// <summary>
        /// Create a copy of this <see cref="SecurityDayData"/>
        /// </summary>
        public SecurityDayData Copy()
        {
            return new SecurityDayData(Date, UnitPrice, ShareNo, NewInvestment, Trade);
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
