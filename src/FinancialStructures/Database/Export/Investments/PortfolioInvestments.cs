﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;

using Effanville.Common.Structure.DataStructures;
using Effanville.Common.Structure.NamingStructures;
using Effanville.Common.Structure.Reporting;
using Effanville.Common.Structure.ReportWriting;
using Effanville.FinancialStructures.Database.Extensions.Values;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Export.Investments
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

                ReportBuilder reportBuilder = new ReportBuilder(DocumentType.Csv, new ReportSettings(true, false, false));
                _ = reportBuilder.WriteTableFromEnumerable(new List<string> { "Date", "Company", "Name", "Investment Amount" }, valuesToWrite, false);

                using (Stream stream = fileSystem.FileStream.Create(filePath, FileMode.Create))
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    fileWriter.WriteLine(reportBuilder.ToString());
                }

                reportLogger?.Log(ReportType.Information, ReportLocation.StatisticsPage.ToString(), $"Successfully exported history to {filePath}.");
            }
            catch (Exception exception)
            {
                reportLogger?.Log(ReportType.Error, ReportLocation.StatisticsPage.ToString(), exception.Message);
            }
        }
    }
}
