using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Text;

using Common.Structure.DataStructures;
using Common.Structure.FileAccess;
using Common.Structure.NamingStructures;
using Common.Structure.Reporting;
using Common.Structure.ReportWriting;

using FinancialStructures.Database.Extensions.Values;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Export.Investments
{
    /// <summary>
    /// Stores all investments in a <see cref="IPortfolio"/>.
    /// </summary>
    public sealed class PortfolioInvestments
    {
        /// <summary>
        /// A list of the name invested in, together with the date and value of the investment.
        /// </summary>
        public List<Labelled<TwoName, DailyValuation>> Investments
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PortfolioInvestments(IPortfolio portfolio, PortfolioInvestmentSettings settings)
        {
            Investments = portfolio.TotalInvestments(settings.TotalsType, settings.Name);
        }

        /// <summary>
        /// Exports the investments to a file.
        /// </summary>
        public void ExportToFile(string filePath)
        {
            ExportToFile(filePath, new FileSystem());
        }

        /// <summary>
        /// Exports the investments to a file.
        /// </summary>
        public void ExportToFile(string filePath, IFileSystem fileSystem, IReportLogger reportLogger = null)
        {
            try
            {
                List<List<string>> valuesToWrite = new List<List<string>>();
                foreach (Labelled<TwoName, DailyValuation> stats in Investments)
                {
                    valuesToWrite.Add(new List<string> { stats.Instance.Day.ToShortDateString(), stats.Label.Company, stats.Label.Name, stats.Instance.Value.ToString() });
                }

                StringBuilder sb = new StringBuilder();
                TableWriting.WriteTableFromEnumerable(sb, ExportType.Csv, new List<string> { "Date", "Company", "Name", "Investment Amount" }, valuesToWrite, false);

                using (Stream stream = fileSystem.FileStream.Create(filePath, FileMode.Create))
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    fileWriter.WriteLine(sb.ToString());
                }

                _ = reportLogger?.LogUseful(ReportType.Information, ReportLocation.StatisticsPage, $"Successfully exported history to {filePath}.");
            }
            catch (Exception exception)
            {
                _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.StatisticsPage, exception.Message);
            }
        }
    }
}
