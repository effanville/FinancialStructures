using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.ReportingStructures;
using System;
using System.Collections.Generic;

namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// General edit functions for a sector.
    /// </summary>
    public partial class SingleValueDataList
    {
        /// <summary>
        /// Compares another security and determines if has same name and company.
        /// </summary>
        internal bool IsEqualTo(SingleValueDataList otherAccount)
        {
            if (otherAccount.GetName() != fName)
            {
                return false;
            }

            if (otherAccount.GetCompany() != fCompany)
            {
                return false;
            }

            return true;
        }

        public int Count()
        {
            return fValues.Count();
        }

        public string GetCompany()
        {
            return fCompany;
        }

        public string GetName()
        {
            return fName;
        }

        public string GetUrl()
        {
            return fUrl;
        }

        public List<DayValue_ChangeLogged> GetDataForDisplay()
        {
            var output = new List<DayValue_ChangeLogged>();
            if (fValues.Any())
            {
                foreach (var datevalue in fValues.GetValuesBetween(fValues.FirstDate(), fValues.LatestDate()))
                {
                    fValues.TryGetValue(datevalue.Day, out double UnitPrice);
                    var thisday = new DayValue_ChangeLogged(datevalue.Day, UnitPrice, false);
                    output.Add(thisday);
                }
            }

            return output;
        }

        public virtual bool EditNameData(string company, string name, string url)
        {
            if (company != fCompany)
            {
                fCompany = company;
            }
            if (name != fName)
            {
                fName = name;
            }
            if (url != fUrl)
            {
                fUrl = url;
            }

            return true;
        }

        /// <summary>
        /// Adds <param name="value"/> to amounts on <param name="date"/> if data doesnt exist.
        /// </summary>
        internal bool TryAddData(DateTime date, double value, ErrorReports reports)
        {
            if (fValues.ValueExists(date, out _))
            {
                reports.AddError("Data already exists.", Location.AddingData);
                return false;
            }

            return fValues.TryAddValue(date, value);
        }

        /// <summary>
        /// Edits value if value exists. Does nothing if it doesn't exist.
        /// </summary>
        public bool TryEditData(DateTime oldDate, DateTime date, double value, ErrorReports reports)
        {
            return fValues.TryEditData(oldDate, date, value, reports);
        }

        /// <summary>
        /// Removes data on <paramref name="date"/> if it exists.
        /// </summary>
        public bool TryDeleteData(DateTime date, ErrorReports reports)
        {
            return fValues.TryDeleteValue(date, reports);
        }
    }
}
