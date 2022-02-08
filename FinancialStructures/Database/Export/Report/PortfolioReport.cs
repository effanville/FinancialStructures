using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;

using Common.Structure.ReportWriting;
using Common.Structure.Reporting;
using Common.Structure.Extensions;

using FinancialStructures.Database.Export.History;
using FinancialStructures.Database.Extensions;
using FinancialStructures.Database.Extensions.Statistics;
using FinancialStructures.NamingStructures;
using System.Collections.Generic;
using FinancialStructures.Database.Statistics;

namespace FinancialStructures.Database.Export.Report
{
    /// <summary>
    /// Contains a report of the entire portfolio.
    /// </summary>
    public sealed class PortfolioReport
    {
        private readonly PortfolioHistory fHistory;
        private readonly IPortfolio fPortfolio;
        private readonly PortfolioReportSettings fSettings;

        private readonly IList<(string, string, string, string)> TotalValues;
        private readonly IList<(string, string, string, string)> RecentTotalValues;
        private readonly List<(string Date, string Value)> CarValues;
        private readonly IList<KeyValuePair<string, decimal>> SectorValues;
        private readonly IList<KeyValuePair<string, decimal>> SecurityCompanyValues;

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public PortfolioReport(IPortfolio portfolio, PortfolioReportSettings settings)
        {
            fPortfolio = portfolio;
            fSettings = settings;
            fHistory = new PortfolioHistory(portfolio, new PortfolioHistorySettings(generateSecurityRates: true, maxIRRIterations: 20));

            TotalValues = fHistory.Snapshots.Select(snap => ($"{snap.Date.Year}-{snap.Date.Month}-{snap.Date.Day}", snap.BankAccValue.TruncateToString(), snap.SecurityValue.TruncateToString(), snap.TotalValue.TruncateToString())).ToList();
            RecentTotalValues = fHistory.Snapshots.Where(rec => rec.Date > DateTime.Today.AddMonths(-6)).Select(snap => ($"{snap.Date.Year}-{snap.Date.Month}-{snap.Date.Day}", snap.BankAccValue.TruncateToString(), snap.SecurityValue.TruncateToString(), snap.TotalValue.TruncateToString())).ToList();

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
        public void ExportToFile(IFileSystem fileSystem, string filePath, PortfolioReportExportSettings settings, IReportLogger logger)
        {
            StringBuilder sb = ExportString(settings);

            try
            {
                using (Stream stream = fileSystem.FileStream.Create(filePath, FileMode.Create))
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    fileWriter.Write(sb.ToString());
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
        public StringBuilder ExportString(PortfolioReportExportSettings settings)
        {
            var exportType = settings.ReportExportType;

            StringBuilder sb = new StringBuilder();

            string title = $"Portfolio Report for {fPortfolio.DatabaseName()} - Statement on {DateTime.Today.ToShortDateString()}";
            TextWriting.CreateHTMLHeader(sb, title, true);
            TextWriting.WriteTitle(sb, exportType, title);
            TextWriting.WriteTitle(sb, exportType, "Total Values", HtmlTag.h2);
            ChartWriting.WriteLineChart(
                sb,
                "line1",
                "Total values over time",
                new[] { "BankAccount", "Security", "Total" },
                TotalValues.Select(value => value.Item1).ToList(),
                new[]
                {
                    TotalValues.Select(value => value.Item2).ToList(),
                    TotalValues.Select(value => value.Item3).ToList(),
                    TotalValues.Select(value => value.Item4).ToList()
                },
                xAxisIsTime: true);

            ChartWriting.WriteLineChart(
                sb,
                "line2",
                "Recent Total values",
                new[] { "BankAccount", "Security", "Total" },
                RecentTotalValues.Select(value => value.Item1).ToList(),
                new[]
                {
                    RecentTotalValues.Select(value => value.Item2).ToList(),
                    RecentTotalValues.Select(value => value.Item3).ToList(),
                    RecentTotalValues.Select(value => value.Item4).ToList()
                },
                xAxisIsTime: true);

            ChartWriting.WriteLineChart(
                sb,
                "line3",
                "Fund return over time",
                new[] { "IRR" },
                CarValues.Select(value => value.Date).ToList(),
                new[] { CarValues.Select(value => value.Value).ToList() },
                xAxisIsTime: true);

            TextWriting.WriteTitle(sb, exportType, "Current Distribution", HtmlTag.h2);

            ChartWriting.WriteBarChart(sb,
                "bar1",
                "Security By Sector",
                "Value",
                SectorValues.Select(x => x.Key).ToList(),
                SectorValues.Select(x => x.Value.TruncateToString()).ToList());

            ChartWriting.WritePieChart(
                sb,
                "pie1",
                "Security by company",
                "Value",
                SecurityCompanyValues.Select(val => val.Key).ToList(),
                SecurityCompanyValues.Select(val => val.Value.TruncateToString()).ToList());

            var companies = fPortfolio.Companies(Account.Security);

            TextWriting.WriteTitle(sb, exportType, "Company Statistics", HtmlTag.h2);
            foreach (string company in companies)
            {
                if (fSettings.DisplayValueFunds && fPortfolio.TotalValue(Totals.Company, new TwoName(company)) > 0m || !fSettings.DisplayValueFunds)
                {
                    TextWriting.WriteTitle(sb, exportType, company, HtmlTag.h3);

                    var stats = fPortfolio.GetStats(DateTime.Today, Totals.SecurityCompany, new TwoName(company), AccountStatisticsHelpers.DefaultSecurityCompanyStats());
                    TableWriting.WriteTable(
                        sb,
                        exportType,
                        new[] { "StatType", "ValueAsObject" },
                        stats[0].Statistics,
                        headerFirstColumn: true);

                    var companyAccounts = fPortfolio.Accounts(Totals.SecurityCompany, new TwoName(company));
                    if (companyAccounts != null && companyAccounts.Count > 1)
                    {
                        ChartWriting.WritePieChart(
                            sb,
                            $"pie{company}",
                            $"{company} securities",
                            "Value",
                            companyAccounts.Select(val => val.Names.Name).ToList(),
                            companyAccounts.Select(acc => acc.LatestValue().Value.TruncateToString()).ToList());
                    }
                    var priceHistory = fHistory.Snapshots.Select(snap => ($"{snap.Date.Year}-{snap.Date.Month}-{snap.Date.Day}", snap.SecurityValues[company], 100 * snap.SecurityTotalCar[company])).Where(val => val.Item2 > 0);

                    ChartWriting.WriteLineChart(
                        sb,
                        $"lineValue{company}",
                        $"{company} value over time",
                        new[] { "Value" },
                        priceHistory.Select(value => value.Item1).ToList(),
                        new[] { priceHistory.Select(value => value.Item2.ToString()).ToList() },
                        xAxisIsTime: true);

                    ChartWriting.WriteLineChart(
                        sb,
                        $"lineReturn{company}",
                        $"{company} Return over time",
                        new[] { "Value" },
                        priceHistory.Select(value => value.Item1).ToList(),
                        new[] { priceHistory.Select(value => value.Item3.ToString()).ToList() },
                        xAxisIsTime: true);
                }
            }

            TextWriting.CreateHTMLFooter(sb);

            return sb;
        }
    }
}
