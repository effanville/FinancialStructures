using System;

namespace FinancialStructures.DataStructures
{
    public class SecurityDayData : IComparable
    {
        public override string ToString()
        {
            return string.Concat(Date.Day.ToString().PadLeft(2, '0'), "/", Date.Month.ToString().PadLeft(2, '0'), "/", Date.Year, ", ", UnitPrice.ToString(), ", ", ShareNo.ToString(), ", ", NewInvestment.ToString());
        }

        public int CompareTo(object obj)
        {
            if (obj is SecurityDayData dailyView)
            {
                return DateTime.Compare(Date, dailyView.Date);
            }
            return 0;
        }

        public SecurityDayData Copy()
        {
            return new SecurityDayData(Date, UnitPrice, ShareNo, NewInvestment, NewValue);
        }

        public SecurityDayData()
        {
            NewValue = true;
            Date = DateTime.Today;
        }

        public SecurityDayData(DateTime date, double unitPrice, double shareNo, double newInvestment, bool newValue = true)
        {
            Date = date;
            UnitPrice = unitPrice;
            ShareNo = shareNo;
            NewInvestment = newInvestment;
            NewValue = newValue;
        }

        /// <summary>
        /// Whether any alterations have been made to thsi.
        /// </summary>
        [Obsolete("This method will soon be deprecated.")]
        public bool NewValue
        {
            get;
            set;
        }

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
    }
}
