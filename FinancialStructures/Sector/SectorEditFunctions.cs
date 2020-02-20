using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.ReportingStructures;
using System;
using System.Collections.Generic;

namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// General edit functions for a sector.
    /// </summary>
    public partial class Sector
    {
        public int Count()
        {
            return fValues.Count();
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

        public bool TryEditNameUrl(string name, string url)
        {
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

        public bool DoesDataExist(DateTime date, out int index)
        {
            return fValues.ValueExists(date, out index);
        }

        public bool TryAddData(DateTime date, double value, ErrorReports reports)
        {
            if (DoesDataExist(date, out int _))
            {
                return false;
            }

            return fValues.TryAddValue(date, value);
        }

        public bool TryEditData(DateTime oldDate, DateTime date, double value, ErrorReports reports)
        {
            if (DoesDataExist(oldDate, out int i))
            {
                return fValues.TryEditData(oldDate, date, value, reports);
            }

            return false;
        }

        public bool TryDeleteData(DateTime date, double value, ErrorReports reports)
        {
            if (value > 0)
            {
                return fValues.TryDeleteValue(date, reports);
            }
            return false;
        }
    }
}
