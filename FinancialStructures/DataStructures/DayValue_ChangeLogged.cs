using System;

namespace FinancialStructures.DataStructures
{
    public class DayValue_ChangeLogged : DailyValuation
    {
        public DayValue_ChangeLogged()
            : base(DateTime.Today, 0.0)
        {
            NewValue = true;
        }

        public override int CompareTo(object obj)
        {
            return base.CompareTo(obj);
        }

        public DayValue_ChangeLogged(DateTime date, double price, bool newValue = true)
            : base(date, price)
        {
            NewValue = newValue;
        }

        public new DayValue_ChangeLogged Copy()
        {
            return new DayValue_ChangeLogged(Day, Value, NewValue);
        }


        public bool NewValue
        {
            get; set;
        }

        public override DateTime Day
        {
            get
            {
                return base.fDate;
            }
            set
            {
                fDate = value;
                NewValue = true;
            }
        }

        public override double Value
        {
            get
            {
                return fValue;
            }
            set
            {
                fValue = value;
                NewValue = true;
            }
        }
    }
}
