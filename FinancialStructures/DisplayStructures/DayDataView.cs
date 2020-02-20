using System;

namespace FinancialStructures.GUIFinanceStructures
{
    public class DayDataView : IComparable
    {
        public int CompareTo(Object obj)
        {
            if (obj is DayDataView dailyView)
            {
                return DateTime.Compare(Date, dailyView.Date);
            }
            return 0;
        }

        public DayDataView Copy()
        {
            return new DayDataView(Date, UnitPrice, ShareNo, NewInvestment, NewValue);
        }

        public DayDataView()
        {
            NewValue = true;
            Date = DateTime.Today;
        }

        public DayDataView(DateTime date, double unitPrice, double shareNo, double newInvestment, bool newValue = true)
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
