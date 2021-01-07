using System;
using System.Collections.Generic;

namespace FinancialStructures.NamingStructures
{
    /// <summary>
    /// A display class for showing names together with a date.
    /// Typically used to show the account information together with last known date.
    /// </summary>
    public class NameCompDate : NameData
    {
        /// <summary>
        /// The date associated with this object.
        /// </summary>
        public DateTime DateToRecord
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor setting all parameters.
        /// </summary>
        public NameCompDate(string company, string name, string currency, string url, HashSet<string> sectors, DateTime date)
            : base(company, name, currency, url, sectors)
        {
            DateToRecord = date;
        }

        internal NameCompDate(string company, string name)
            : base(company, name)
        {
            DateToRecord = DateTime.MinValue;
        }

        /// <summary>
        /// Provides an exact copy of the <see cref="NameCompDate"/>.
        /// </summary>
        public new NameCompDate Copy()
        {
            return new NameCompDate(Company, Name, Currency, Url, Sectors, DateToRecord);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is NameCompDate otherName)
            {
                if (DateToRecord.Equals(otherName.DateToRecord))
                {
                    return base.Equals(otherName);
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return DateToRecord.GetHashCode() * 356 + base.GetHashCode();
        }
    }
}
