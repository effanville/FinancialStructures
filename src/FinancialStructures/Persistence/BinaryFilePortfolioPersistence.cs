using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

using Effanville.Common.Structure.FileAccess;
using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.Database.Implementation;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.FinanceStructures.Implementation;
using Effanville.FinancialStructures.Persistence.Xml;

namespace Effanville.FinancialStructures.Persistence
{
    public sealed class BinaryFilePortfolioPersistence : IPersistence<IPortfolio>
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
                reportLogger?.Info(nameof(BinaryFilePortfolioPersistence), "Options for loading from Binary file not of correct type.");
                return false;
            }

            IFileSystem fileSystem = binaryFileOptions.FileSystem;
            string filePath = binaryFileOptions.FilePath;
            if (!fileSystem.File.Exists(filePath))
            {
                reportLogger?.Info(nameof(BinaryFilePortfolioPersistence), "Loaded Empty New Portfolio.");
                return false;
            }

            if (portfolio is not Portfolio portfolioImpl)
            {
                return false;
            }

            string output;
            using (Stream file = fileSystem.FileStream.New(filePath, FileMode.Open, FileAccess.Read))
            using (StreamReader streamReader = new StreamReader(file))
            {
                output = streamReader.ReadToEnd();
            }

            byte[] byteInput = Convert.FromBase64String(output);

            MemoryStream stream = new MemoryStream(byteInput);
            AllData database = XmlFileAccess.ReadFromStream<AllData>(stream, out string error);
            if (database != null)
            {
                portfolioImpl.Clear();
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
                reportLogger?.Info(nameof(BinaryFilePortfolioPersistence), $"Loaded new database from {filePath}");
            }
            else
            {
                reportLogger?.Error(nameof(BinaryFilePortfolioPersistence), $" Failed to load new database from {filePath}. {error}.");
            }

            foreach (ISecurity security in portfolio.Funds)
            {
                var sec = (Security)security;
                sec.EnsureOnLoadDataConsistency();
            }

            foreach (ISecurity security in portfolio.Pensions)
            {
                var sec = (Security)security;
                sec.EnsureOnLoadDataConsistency();
            }

            portfolioImpl.OnNewPortfolio(this, new PortfolioEventArgs(true));
            return true;
        }

        public bool Save(IPortfolio portfolio, PersistenceOptions options, IReportLogger reportLogger = null)
        {
            if (options is not BinaryFilePersistenceOptions binaryFileOptions)
            {
                reportLogger?.Info(nameof(BinaryFilePortfolioPersistence), "Options for loading from Xml file not of correct type.");
                return false;
            }

            IFileSystem fileSystem = binaryFileOptions.FileSystem;
            string filePath = binaryFileOptions.FilePath;
            if (portfolio is not Portfolio portfolioImpl)
            {
                reportLogger?.Error(nameof(BinaryFilePortfolioPersistence), "Attempted to save a StockExchange that was not of the correct type.");
                return false;
            }

            AllData toSave = new AllData(portfolioImpl, null);

            var stream = new MemoryStream();
            XmlFileAccess.WriteToStream(stream, toSave, out string error);
            if (error != null)
            {
                reportLogger?.Error(nameof(BinaryFilePortfolioPersistence), $"Failed to save database: {error}");
                return false;
            }

            byte[] bytes = stream.ToArray();
            string base64 = Convert.ToBase64String(bytes);
            using (Stream file = fileSystem.FileStream.New(filePath, FileMode.Create, FileAccess.Write)) 
            using (StreamWriter streamWriter = new StreamWriter(file))
            {
                streamWriter.Write(base64);
            }

            portfolioImpl.Saving();
            reportLogger?.Info(nameof(BinaryFilePortfolioPersistence), $"Saved Database at {filePath}");
            return true;
        }
    }
}