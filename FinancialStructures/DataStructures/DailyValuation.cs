using System;

namespace FinancialStructures.DataStructures
{
    /// <summary>
    /// Holds a date and a value to act as the value on that day.
    /// </summary>
    public class DailyValuation : IComparable
    {
        /// <summary>
        /// Appends date in UK format with value, separated by a comma.
        /// </summary>
        public override string ToString()
        {
            return string.Concat(Day.Day.ToString().PadLeft(2, '0'), "/", Day.Month.ToString().PadLeft(2, '0'), "/", Day.Year, ", ", Value.ToString());
        }

        /// <summary>
        /// Method of comparison. Compares dates.
        /// </summary>
        public virtual int CompareTo(object obj)
        {
            DailyValuation a = (DailyValuation)obj;
            return DateTime.Compare(Day, a.Day);
        }

        /// <summary>
        /// Returns a copy of the specified valuation
        /// </summary>
        /// <returns></returns>
        public DailyValuation Copy()
        {
            return new DailyValuation(Day, Value);
        }

        public void SetData(DateTime date, double value)
        {
            Day = date;
            Value = value;
        }

        public void SetDay(DateTime date)
        {
            Day = date;
        }

        public void SetValue(double value)
        {
            Value = value;
        }

        protected DateTime fDate;

        /// <summary>
        /// The date for the valuation
        /// </summary>
        public virtual DateTime Day
        {
            get { return fDate; }
            set { fDate = value; }
        }

        protected double fValue;

        /// <summary>
        /// The specific valuation
        /// </summary>
        public virtual double Value 
        {
            get { return fValue; }
            set { fValue = value; } 
        }

        /// <summary>
        /// empty constructor.
        /// </summary>
        protected DailyValuation()
        {
        }

        /// <summary>
        /// Standard constructor.
        /// </summary>
        public DailyValuation(DateTime idealDate, double idealValue)
        {
            Day = idealDate;
            Value = idealValue;
        }

        public DailyValuation(DailyValuation dailyValue)
            : this(dailyValue.Day, dailyValue.Value)
        { }
    }
}
