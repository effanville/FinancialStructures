using System;
using System.Collections.Generic;

namespace FinancialStructures.GUIFinanceStructures
{
    public class NameCompDate : NameData
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

        public NameCompDate(string company, string name, string currency, string url, List<string> sectors, DateTime date, bool newValue) : base(company, name, currency, url, sectors, newValue)
        {
            fDateToRecord = date;
        }

        public NameCompDate(string company, string name, string currency, string url, bool newValue) : base(company, name, currency, url, newValue)
        {
            fDateToRecord = DateTime.MinValue;
        }
    }
}
