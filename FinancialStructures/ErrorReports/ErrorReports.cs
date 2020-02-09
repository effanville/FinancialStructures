using System.Collections.Generic;
using System.Linq;

namespace FinancialStructures.ReportingStructures
{
    /// <summary>
    /// Structure of reports to tell the user what is happening
    /// </summary>
    public class ErrorReports
    {
        public ErrorReports()
        {
            fReports = new List<ErrorReport>();
        }
        public int Count()
        {
            return fReports.Count();
        }

        public bool Any()
        {
            return fReports.Any();
        }
        /// <summary>
        /// Instantiates the 
        /// </summary>
        public bool Configure()
        {
            fReports = new List<ErrorReport>();
            return true;
        }

        private List<ErrorReport> fReports;

        public void AddReports(ErrorReports reports)
        {
            fReports.AddRange(reports.GetReports());
            //fReports.Sort();
        }

        /// <summary>
        /// Adds a report of any type to the existing list 
        /// </summary>
        private void AddGeneralReport(ReportType type, Location location, string newReport)
        {
            fReports.Add(new ErrorReport(type, location, newReport));
        }

        /// <summary>
        /// Adds a report to the existing list 
        /// </summary>
        public void AddReport(string newReport, Location location = Location.Unknown)
        {
            AddGeneralReport(ReportType.Report, location, newReport);
        }

        /// <summary>
        /// Adds an Error report to the existing list 
        /// </summary>
        public void AddError(string newReport, Location location = Location.Unknown)
        {
            AddGeneralReport(ReportType.Error, location, newReport);
        }

        /// <summary>
        /// Adds a Warning report to the existing list 
        /// </summary>
        public void AddWarning(string newReport, Location location = Location.Unknown)
        {
            AddGeneralReport(ReportType.Warning, location, newReport);
        }

        /// <summary>
        /// Function to obtain 
        /// </summary>
        /// <returns>Currently held reports</returns>
        public List<ErrorReport> GetReports()
        {
            return fReports;
        }

        /// <summary>
        /// Returns a list of reports from the specified location
        /// </summary>
        public List<ErrorReport> GetReports(Location location)
        {
            return fReports.Where(report => report.ErrorLocation == location).ToList();
        }

        /// <summary>
        /// Returns a list of reports with the specified location
        /// </summary>
        public List<ErrorReport> GetReports(ReportType reportType)
        {
            return fReports.Where(report => report.ErrorType == reportType).ToList();
        }

        /// <summary>
        /// Removes element at index <param name="i"/>
        /// </summary>
        public void RemoveReport(int i)
        {
            if (i >= 0 && i < fReports.Count())
            {
                fReports.RemoveAt(i);
            }
        }

        public void Clear()
        {
            fReports.Clear();
        }
    }
}
