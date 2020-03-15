using System;

namespace FinancialStructures.ReportLogging
{
    /// <summary>
    /// Collection of standard reporting mechanisms.
    /// </summary>
    public class LogReporter
    {
        /// <summary>
        /// Log an arbitrary message.
        /// Parameter order is
        /// Level of detail for display.
        /// Type of report.
        /// Location of report.
        /// Message for the report.
        /// </summary>
        public Action<string, string, string, string> LogDetailed;

        /// <summary>
        /// Log an arbitrary message.
        /// Parameter order is
        /// Type of report.
        /// Location of report.
        /// Message for the report.
        /// </summary>
        public Action<string, string, string> Log;

        /// <summary>
        /// Log an error message.
        /// Parameter order is
        /// Location of report.
        /// Message for the report.
        /// </summary>
        public Action<string, string> LogError;

        /// <summary>
        /// Constructor for reporting mechanisms. Parameter addReport is the report callback mechanism.
        /// </summary>
        public LogReporter(Action<string, string, string, string> addReport)
        {
            LogDetailed = (detailLevel, type, location, message) => addReport(detailLevel, type, location, message); ;
            Log = (type, location, message) => addReport("Useful", type, location, message);
            LogError = (location, message) => addReport("Useful", "Error", location, message);
        }
    }
}
