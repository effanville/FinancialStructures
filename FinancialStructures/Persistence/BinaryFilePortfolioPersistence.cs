using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

using Common.Structure.FileAccess;
using Common.Structure.Reporting;

using FinancialStructures.Database;
using FinancialStructures.Database.Implementation;
using FinancialStructures.FinanceStructures;
using FinancialStructures.FinanceStructures.Implementation;
using FinancialStructures.Persistence.Xml;

namespace FinancialStructures.Persistence
{
    public sealed class BinaryFilePortfolioPersistence: IPersistence<IPortfolio>
    {
        public IPortfolio Load(PersistenceOptions options, IReportLogger reportLogger = null)
        {
            Portfolio portfolio = new Portfolio();
            if (!Load(portfolio, options, reportLogger))
            {
                return null;
            }

            return portfolio;
        }

        public bool Load(IPortfolio portfolio, PersistenceOptions options, IReportLogger reportLogger = null)
        {
            if (options is not BinaryFilePersistenceOptions binaryFileOptions)
            {
                reportLogger?.Log(ReportType.Information, ReportLocation.Loading.ToString(),
                    "Options for loading from Binary file not of correct type.");
                return false;
            }

            IFileSystem fileSystem = binaryFileOptions.FileSystem;
            string filePath = binaryFileOptions.FilePath;
            if (!fileSystem.File.Exists(filePath))
            {
                reportLogger?.Log(ReportType.Information, ReportLocation.Loading.ToString(),
                    "Loaded Empty New Portfolio.");
                return false;
            }

            if (portfolio is not Portfolio portfolioImpl)
            {
                return false;
            }

            string output;
            using (Stream file = fileSystem.FileStream.Create(filePath, FileMode.Open, FileAccess.Read))
            using(StreamReader streamReader = new StreamReader(file))
            {
                output = streamReader.ReadToEnd();
            }

            byte[] byteInput = Convert.FromBase64String(output);
            
            MemoryStream stream = new MemoryStream(byteInput);
            AllData database = XmlFileAccess.ReadFromStream<AllData>(stream, out string error);
            if (database != null)
            {
                database.MyFunds.Set(portfolioImpl);

                if (!database.MyFunds.BenchMarks.Any())
                {
                    foreach (var benchmark in database.myBenchMarks)
                    {
                        portfolioImpl.AddBenchMark(new Sector(benchmark.Names, benchmark.Values));
                    }
                }

                portfolioImpl.WireDataChangedEvents();
                portfolioImpl.Name = fileSystem.Path.GetFileNameWithoutExtension(filePath);
                portfolioImpl.Saving();
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Information, ReportLocation.Loading,
                    $"Loaded new database from {filePath}");
            }
            else
            {
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Loading,
                    $" Failed to load new database from {filePath}. {error}.");
            }

            foreach (ISecurity security in portfolio.Funds)
            {
                var sec = (Security)security;
                sec.EnsureOnLoadDataConsistency();
            }

            portfolioImpl.OnPortfolioChanged(this, new PortfolioEventArgs());
            return true;
        }

        public bool Save(IPortfolio portfolio, PersistenceOptions options, IReportLogger reportLogger = null)
        {
            if (options is not BinaryFilePersistenceOptions binaryFileOptions)
            {
                reportLogger?.Log(ReportType.Information, ReportLocation.Loading.ToString(),
                    "Options for loading from Xml file not of correct type.");
                return false;
            }

            IFileSystem fileSystem = binaryFileOptions.FileSystem;
            string filePath = binaryFileOptions.FilePath;
            if (portfolio is not Portfolio portfolioImpl)
            {
                reportLogger?.Log(ReportType.Error, ReportLocation.Saving.ToString(),
                    "Attempted to save a StockExchange that was not of the correct type.");
                return false;
            }

            AllData toSave = new AllData(portfolioImpl, null);

            var stream = new MemoryStream();
            XmlFileAccess.WriteToStream(stream, toSave, out string error);
            if (error != null)
            {
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Saving,
                    $"Failed to save database: {error}");
                return false;
            }

            byte[] bytes = stream.ToArray();
            string base64 = Convert.ToBase64String(bytes);
            using (Stream file = fileSystem.FileStream.Create(filePath, FileMode.Create, FileAccess.Write)) 
            using (var streamWriter = new StreamWriter(file))
            {
                streamWriter.Write(base64);
            }

            portfolioImpl.Saving();
            _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Information, ReportLocation.Saving,
                $"Saved Database at {filePath}");
            return true;
        }
    }
}