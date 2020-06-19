using System;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;

namespace FinancialStructures.DataStructures
{
    public class DayValue_Named : DailyValuation
    {
        public override int CompareTo(object obj)
        {
            if (obj is DayValue_Named value)
            {
                return Names.CompareTo(value.Names);
            }

            return 0;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Names.ToString() + "-" + base.ToString();
        }

        /// <summary>
        /// Names associated to the values.
        /// </summary>
        public TwoName Names
        {
            get;
            set;
        }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public DayValue_Named() : base()
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DayValue_Named(string company, string name, DateTime day, double value)
            : base(day, value)
        {
            Names = new TwoName(company, name);
        }

        /// <summary>
        /// Constructor to create an instance from a base class instance.
        /// </summary>
        public DayValue_Named(string company, string name, DailyValuation toAddOnto)
            : this(company, name, toAddOnto.Day, toAddOnto.Value)
        {
        }
    }
}
