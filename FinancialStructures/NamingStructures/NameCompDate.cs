using System;
using System.Collections.Generic;

namespace FinancialStructures.NamingStructures
{
    public class NameCompDate : NameData
    {
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

        public override int GetHashCode()
        {
            return DateToRecord.GetHashCode() * 356 + base.GetHashCode();
        }

        private DateTime fDateToRecord;
        public DateTime DateToRecord
        {
            get
            {
                return fDateToRecord;
            }
            set
            {
                fDateToRecord = value;
            }
        }

        public NameCompDate() : base()
        {
        }

        public NameCompDate(string company, string name, string currency, string url, HashSet<string> sectors, DateTime date) : base(company, name, currency, url, sectors)
        {
            fDateToRecord = date;
        }

        public NameCompDate(string company, string name, string currency, string url) : base(company, name, currency, url)
        {
            fDateToRecord = DateTime.MinValue;
        }

        public NameCompDate(string company, string name) : base(company, name)
        {
            fDateToRecord = DateTime.MinValue;
        }

        public new NameCompDate Copy()
        {
            return new NameCompDate(Company, Name, Currency, Url, Sectors, DateToRecord);
        }
    }
}
