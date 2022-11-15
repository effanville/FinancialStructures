using Common.Structure.ReportWriting;

namespace FinancialStructures.Database.Export.Report
{
    public sealed partial class PortfolioReport
    {
        /// <summary>
        /// Contains settings for the export of a PortfolioReport.
        /// </summary>
        public sealed class ExportSettings
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
            public ExportSettings(DocumentType reportExportType)
            {
                ReportExportType = reportExportType;
            }

            /// <summary>
            /// Generate the default settings.
            /// </summary>
            public static ExportSettings Default()
            {
                return new ExportSettings(DocumentType.Html);
            }
        }
    }
}
