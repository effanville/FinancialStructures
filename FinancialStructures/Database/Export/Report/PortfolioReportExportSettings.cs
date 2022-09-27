using Common.Structure.ReportWriting;

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
        public DocumentType ReportExportType
        {
            get;
        }

        /// <summary>
        /// Generate an instance.
        /// </summary>
        public PortfolioReportExportSettings(DocumentType reportExportType)
        {
            ReportExportType = reportExportType;
        }

        /// <summary>
        /// Generate the default settings.
        /// </summary>
        public static PortfolioReportExportSettings DefaultSettings()
        {
            return new PortfolioReportExportSettings(DocumentType.Html);
        }
    }
}
