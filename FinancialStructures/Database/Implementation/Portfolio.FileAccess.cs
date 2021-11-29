using System.IO.Abstractions;
using System.Linq;
using Common.Structure.FileAccess;
using Common.Structure.Reporting;
using FinancialStructures.FinanceStructures.Implementation;
using FinancialStructures.SavingClasses;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public void LoadPortfolio(string filePath, IFileSystem fileSystem, IReportLogger reportLogger = null)
        {
            if (!string.IsNullOrWhiteSpace(filePath) && fileSystem.File.Exists(filePath))
            {
                AllData database = XmlFileAccess.ReadFromXmlFile<AllData>(fileSystem, filePath, out string error);
                if (database != null)
                {
                    SetFrom(database.MyFunds);

                    if (!database.MyFunds.BenchMarks.Any())
                    {
                        SetBenchMarks(database.myBenchMarks);
                    }

                    WireDataChangedEvents();
                    Saving();
                    _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Information, ReportLocation.Loading, $"Loaded new database from {filePath}");
                }
                else
                {
                    _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Loading, $" Failed to load new database from {filePath}. {error}.");
                }

                foreach (Security sec in FundsThreadSafe)
                {
                    sec.EnsureOnLoadDataConsistency();
                }

                OnPortfolioChanged(this, new PortfolioEventArgs(changedPortfolio: true));
                return;
            }

            _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Information, ReportLocation.Loading, "Loaded Empty New Database.");

            Clear();
        }

        /// <inheritdoc/>
        public void SavePortfolio(string filePath, IFileSystem fileSystem, IReportLogger reportLogger = null)
        {
            AllData toSave = new AllData(this, null);

            if (filePath != null)
            {
                XmlFileAccess.WriteToXmlFile(fileSystem, filePath, toSave, out string error);
                if (error != null)
                {
                    _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Saving, $"Failed to save database: {error}");
                    return;
                }

                Saving();
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Information, ReportLocation.Saving, $"Saved Database at {filePath}");
            }
        }
    }
}
