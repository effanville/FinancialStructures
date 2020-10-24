using System;
using System.IO;
using System.Linq;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.SavingClasses;
using StructureCommon.FileAccess;
using StructureCommon.Reporting;

namespace FinancialStructures.Database
{
    public partial class Portfolio
    {
        /// <inheritdoc />
        public void Clear()
        {
            SetFilePath("");
            BaseCurrency = "";
            IsAlteredSinceSave = false;
            Funds.Clear();
            BenchMarks.Clear();
            Currencies.Clear();
            BankAccounts.Clear();
        }

        /// <inheritdoc/>
        public void LoadPortfolio(string filePath, IReportLogger reportLogger = null)
        {
            if (File.Exists(filePath))
            {
                AllData database = XmlFileAccess.ReadFromXmlFile<AllData>(filePath, out string error);
                if (database != null)
                {
                    CopyData(database.MyFunds);
                    if (!database.MyFunds.BenchMarks.Any())
                    {
                        SetBenchMarks(database.myBenchMarks);
                    }

                    WireDataChangedEvents();
                    OnPortfolioChanged(this, new PortfolioEventArgs());
                    Saving();
                    _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Report, ReportLocation.Loading, $"Loaded new database from {filePath}");
                }
                else
                {
                    _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Loading, $" Failed to load new database from {filePath}. {error}.");
                }

                return;
            }

            _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Report, ReportLocation.Loading, "Loaded Empty New Database.");

            CopyData(new Portfolio());
            WireDataChangedEvents();
            OnPortfolioChanged(this, new PortfolioEventArgs());
            Saving();
        }

        /// <inheritdoc/>
        public void SavePortfolio(string filePath, IReportLogger reportLogger = null)
        {
            AllData toSave = new AllData(this, null);

            if (filePath != null)
            {
                XmlFileAccess.WriteToXmlFile(filePath, toSave, out string error);
                if (error != null)
                {
                    _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Saving, $"Failed to save database: {error}");
                    return;
                }

                Saving();
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Report, ReportLocation.Saving, $"Saved Database at {filePath}");
            }
        }
    }
}
