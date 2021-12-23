using Common.Structure.FileAccess;

namespace FinancialStructures.Database.Export.Report
{
    /// <summary>
    /// Contains settings for the export of a PortfolioReport.
    /// </summary>
    public sealed class PortfolioReportExportSettings
    {
        /// <summary>
        /// The type of file to export to.
        /// </summary>
        public ExportType ReportExportType
        {
            get;
        }

        /// <summary>
        /// Generate an instance.
        /// </summary>
        public PortfolioReportExportSettings(ExportType reportExportType)
        {
            ReportExportType = reportExportType;
        }

        /// <summary>
        /// Generate the default settings.
        /// </summary>
        public static PortfolioReportExportSettings DefaultSettings()
        {
            return new PortfolioReportExportSettings(ExportType.Html);
        }
    }
}
