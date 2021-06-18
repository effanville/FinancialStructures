using System;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;

namespace FinancialStructures.DataStructures
{
    /// <summary>
    /// Wraps a <see cref="TwoName"/> around a <see cref="DailyValuation"/>.
    /// </summary>
    public class DayValue_Named : DailyValuation, IEquatable<DayValue_Named>
    {
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

        /// <inheritdoc/>
        public override int CompareTo(object obj)
        {
            if (obj is DayValue_Named value)
            {
                return Names.CompareTo(value.Names) + base.CompareTo(value);
            }

            return 0;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Names.ToString() + "-" + base.ToString();
        }

        /// <inheritdoc/>
        public bool Equals(DayValue_Named other)
        {
            if (Names.Equals(other.Names) && Value.Equals(other.Value) && Day.Equals(other.Day))
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is DayValue_Named other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = 23 * hashCode + Names.GetHashCode();
            hashCode = 23 * hashCode + Value.GetHashCode();
            hashCode = 23 * hashCode + Day.GetHashCode();
            return hashCode;
        }
    }
}
