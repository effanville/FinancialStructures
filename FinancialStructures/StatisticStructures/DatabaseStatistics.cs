using FinancialStructures.NamingStructures;
using System;

namespace FinancialStructures.StatisticStructures
{
    /// <summary>
    /// Holds statistics about an account pertaining to the records stored in the account.
    /// </summary>
    public class DatabaseStatistics : TwoName
    {
        private DateTime fFirstDate;
        public DateTime FirstDate
        {
            get { return fFirstDate; }
            set { fFirstDate = value; }
        }

        private DateTime fLatestDate;
        public DateTime LatestDate
        {
            get { return fLatestDate; }
            set { fLatestDate = value; }
        }

        private int fNumEntries;

        public int NumEntries
        {
            get { return fNumEntries; }
            set { fNumEntries = value; }
        }

        private double fEntryYearDensity;

        public double EntryYearDensity
        {
            get { return fEntryYearDensity; }
            set { fEntryYearDensity = value; }
        }


        public DatabaseStatistics(string company, string name, DateTime firstDate, DateTime latestDate, int numEntries, double density)
            : base(company, name)
        {
            FirstDate = firstDate;
            LatestDate = latestDate;
            NumEntries = numEntries;
            EntryYearDensity = density;
        }
    }
}
