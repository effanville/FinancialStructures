using System;

namespace FinancialStructures.DataStructures
{
    public class SecurityDayData : IComparable
    {
        public override string ToString()
        {
            return string.Concat(Date.Day.ToString().PadLeft(2, '0'), "/", Date.Month.ToString().PadLeft(2, '0'), "/", Date.Year, ", ", UnitPrice.ToString(), ", ", ShareNo.ToString(), ", ", NewInvestment.ToString());
        }

        public int CompareTo(Object obj)
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
            Value = UnitPrice * ShareNo;
            NewInvestment = newInvestment;
            NewValue = newValue;
        }

        public bool NewValue { get; set; }

        public DateTime Date { get; set; }
        public double UnitPrice { get; set; }

        public double ShareNo { get; set; }

        public double Value { get; set; }

        public double NewInvestment { get; set; }
    }
}
