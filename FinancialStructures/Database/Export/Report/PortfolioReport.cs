using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

using Common.Structure.ReportWriting;
using Common.Structure.Reporting;
using Common.Structure.Extensions;

using FinancialStructures.Database.Export.History;
using FinancialStructures.Database.Extensions;
using FinancialStructures.Database.Extensions.Statistics;
using FinancialStructures.NamingStructures;
using System.Collections.Generic;
using FinancialStructures.Database.Statistics;
using FinancialStructures.Database.Extensions.Values;

namespace FinancialStructures.Database.Export.Report
{
    /// <summary>
    /// Contains a report of the entire portfolio.
    /// </summary>
    public sealed partial class PortfolioReport
    {
        private readonly PortfolioHistory fHistory;
        private readonly IPortfolio fPortfolio;
        private readonly Settings fSettings;

        private readonly IList<List<string>> TotalValues;
        private readonly IList<List<string>> RecentTotalValues;
        private readonly List<(string Date, string Value)> CarValues;
        private readonly IList<KeyValuePair<string, decimal>> SectorValues;
        private readonly IList<KeyValuePair<string, decimal>> SecurityCompanyValues;

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public PortfolioReport(IPortfolio portfolio, Settings settings)
        {
            fPortfolio = portfolio;
            fSettings = settings;
            fHistory = new PortfolioHistory(portfolio, new PortfolioHistory.Settings(generateSecurityRates: true, maxIRRIterations: 20));

            TotalValues = fHistory.Snapshots.Select(snap => snap.ExportValues("yyyy-MM-dd", includeSecurityValues: false, false, false, false)).ToList();
            RecentTotalValues = fHistory.Snapshots.Where(rec => rec.Date > DateTime.Today.AddMonths(-6)).Select(snap => snap.ExportValues("yyyy-MM-dd", includeSecurityValues: false, false, false, false)).ToList();

            CarValues = fHistory.Snapshots.Where(s => s.TotalSecurityIRR > 0).Select(snap => ($"{snap.Date.Year}-{snap.Date.Month}-{snap.Date.Day}", snap.TotalSecurityIRR.TruncateToString(5))).ToList();
            SectorValues = fHistory.Snapshots[fHistory.Snapshots.Count - 1].SectorValues.Where(x => x.Value > 0).ToList();
            SecurityCompanyValues = fHistory.Snapshots[fHistory.Snapshots.Count - 1].SecurityValues.Where(x => x.Value > 0).ToList();
        }

        /// <summary>
        /// Exports the statistics to a file.
        /// </summary>
        /// <param name="fileSystem">The file system interface to use.</param>
        /// <param name="filePath">The path exporting to.</param>
        /// <param name="settings">Various options for the export.</param>
        /// <param name="logger">Returns information on success or failure.</param>
        public void ExportToFile(IFileSystem fileSystem, string filePath, ExportSettings settings, IReportLogger logger)
        {
            ReportBuilder reportBuilder = ExportString(settings);

            try
            {
                using (Stream stream = fileSystem.FileStream.Create(filePath, FileMode.Create))
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    fileWriter.Write(reportBuilder.ToString());
                }
            }
            catch (IOException exception)
            {
                _ = logger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.StatisticsPage, $"Error in exporting statistics page: {exception.Message}.");
                return;
            }

            _ = logger.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.StatisticsPage, "Successfully exported statistics page.");
        }

        /// <summary>
        /// Generates the export string for the report.
        /// </summary>
        public ReportBuilder ExportString(ExportSettings settings)
        {
            var exportType = settings.ReportExportType;

            ReportBuilder reportBuilder = new ReportBuilder(settings.ReportExportType, new ReportSettings(true, false, true));

            string title = $"Portfolio Report for {fPortfolio.DatabaseName()} - Statement on {DateTime.Today.ToShortDateString()}";
            _ = reportBuilder.WriteHeader(title)
                .WriteTitle(title, DocumentElement.h1)
                .WriteTitle("Total Values", DocumentElement.h2)
                .WriteLineChart(
                "line1",
                "Total values over time",
                new[] { "BankAccount", "Security", "Pension", "Asset", "Total" },
                TotalValues.Select(value => value[0]).ToList(),
                new[]
                {
                    TotalValues.Select(value => value[2]).ToList(),
                    TotalValues.Select(value => value[3]).ToList(),
                    TotalValues.Select(value => value[5]).ToList(),
                    TotalValues.Select(value => value[4]).ToList(),
                    TotalValues.Select(value => value[1]).ToList()
                },
                xAxisIsTime: true);

            _ = reportBuilder.WriteLineChart(
                "line2",
                "Recent Total values",
                new[] { "BankAccount", "Security", "Pension", "Asset", "Total" },
                RecentTotalValues.Select(value => value[0]).ToList(),
                new[]
                {
                    RecentTotalValues.Select(value => value[2]).ToList(),
                    RecentTotalValues.Select(value => value[3]).ToList(),
                    RecentTotalValues.Select(value => value[5]).ToList(),
                    RecentTotalValues.Select(value => value[4]).ToList(),
                    RecentTotalValues.Select(value => value[1]).ToList()
                },
                xAxisIsTime: true);

            _ = reportBuilder.WriteLineChart(
                "line3",
                "Fund return over time",
                new[] { "IRR" },
                CarValues.Select(value => value.Date).ToList(),
                new[] { CarValues.Select(value => value.Value).ToList() },
                xAxisIsTime: true);

            _ = reportBuilder.WriteTitle("Current Distribution", DocumentElement.h2)
                .WriteBarChart(
                "bar1",
                "Security By Sector",
                "Value",
                SectorValues.Select(x => x.Key).ToList(),
                SectorValues.Select(x => x.Value.TruncateToString()).ToList());

            _ = reportBuilder.WritePieChart(
                "pie1",
                "Security by company",
                "Value",
                SecurityCompanyValues.Select(val => val.Key).ToList(),
                SecurityCompanyValues.Select(val => val.Value.TruncateToString()).ToList());

            var companies = fPortfolio.Companies(Account.All);

            _ = reportBuilder.WriteTitle("Company Statistics", DocumentElement.h2);
            foreach (string company in companies)
            {
                if (fSettings.DisplayValueFunds && fPortfolio.TotalValue(Totals.Company, new TwoName(company)) > 0m || !fSettings.DisplayValueFunds)
                {
                    _ = reportBuilder.WriteTitle(company, DocumentElement.h3);

                    var stats = fPortfolio.GetStats(DateTime.Today, Totals.Company, new TwoName(company), AccountStatisticsHelpers.DefaultSecurityCompanyStats());
                    _ = reportBuilder.WriteTable(
                        new[] { "StatType", "ValueAsObject" },
                        stats[0].Statistics,
                        headerFirstColumn: true);

                    var companyAccounts = fPortfolio.Accounts(Totals.Company, new TwoName(company));
                    if (companyAccounts != null && companyAccounts.Count > 1)
                    {
                        _ = reportBuilder.WritePieChart(
                            $"pie{company}",
                            $"{company} securities",
                            "Value",
                            companyAccounts.Select(val => val.Names.Name).ToList(),
                            companyAccounts.Select(acc => acc.LatestValue().Value.TruncateToString()).ToList());
                    }
                    var priceHistory = fHistory.Snapshots.Select(snap => snap.GetCompanyStrings(company)).Where(val => val.Value > 0);

                    _ = reportBuilder.WriteLineChart(
                        $"lineValue{company}",
                        $"{company} value over time",
                        new[] { "Value" },
                        priceHistory.Select(value => value.Date).ToList(),
                        new[] { priceHistory.Select(value => value.Value.ToString()).ToList() },
                        xAxisIsTime: true);

                    _ = reportBuilder.WriteLineChart(
                        $"lineReturn{company}",
                        $"{company} Return over time",
                        new[] { "IRR" },
                        priceHistory.Select(value => value.Date).ToList(),
                        new[] { priceHistory.Select(value => value.TotalIRR.ToString()).ToList() },
                        xAxisIsTime: true);
                }
            }

            _ = reportBuilder.WriteFooter();

            return reportBuilder;
        }
    }
}
