using System;
using System.Collections.Generic;

namespace FinancialStructures.NamingStructures
{
    public class NameCompDate : NameData_ChangeLogged
    {
        private DateTime fDateToRecord;
        public DateTime DateToRecord
        {
            get { return fDateToRecord; }
            set { fDateToRecord = value; }
        }

        public NameCompDate() : base()
        {
        }

        public NameCompDate(string company, string name, string currency, string url, HashSet<string> sectors, DateTime date) : base(company, name, currency, url, sectors, false)
        {
            fDateToRecord = date;
        }

        public NameCompDate(string company, string name, string currency, string url) : base(company, name, currency, url, false)
        {
            fDateToRecord = DateTime.MinValue;
        }
    }
}
