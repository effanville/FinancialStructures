using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using Common.Structure.Reporting;
using Common.Structure.ReportWriting;
using FinancialStructures.Database.Extensions;
using FinancialStructures.Database.Extensions.Statistics;
using FinancialStructures.Database.Statistics;
using FinancialStructures.DataStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Export.Statistics
{
    /// <summary>
    /// Container for statistics on a portfolio.
    /// </summary>
    public class PortfolioStatistics
    {
        private readonly string fDatabaseName;

        /// <summary>
        /// Totals of different types held in portfolio.
        /// </summary>
        internal List<AccountStatistics> PortfolioTotals
        {
            get;
            private set;
        }

        /// <summary>
        /// A list of all securities and their statistics.
        /// </summary>
        internal List<AccountStatistics> SecurityStats
        {
            get;
            private set;
        }

        /// <summary>
        /// List of statistics of each company performance
        /// </summary>
        internal List<AccountStatistics> SecurityCompanyStats
        {
            get;
            private set;
        }

        /// <summary>
        /// List of statistics of each company performance
        /// </summary>
        public List<AccountStatistics> SecurityTotalStats
        {
            get;
            private set;
        }

        /// <summary>
        /// Each specified sectors performance.
        /// </summary>
        public List<AccountStatistics> SectorStats
        {
            get;
            private set;
        }

        /// <summary>
        /// Value held in each bank account.
        /// </summary>
        internal List<AccountStatistics> BankAccountStats
        {
            get;
            private set;
        }

        /// <summary>
        /// Statistics for each company holding bank accounts.
        /// </summary>
        internal List<AccountStatistics> BankAccountCompanyStats
        {
            get;
            private set;
        }

        /// <summary>
        /// Total statistics for each BankAccount.
        /// </summary>
        internal List<AccountStatistics> BankAccountTotalStats
        {
            get;
            private set;
        }

        /// <summary>
        /// Value held in each asset.
        /// </summary>
        internal List<AccountStatistics> AssetStats
        {
            get;
            private set;
        }

        /// <summary>
        /// Statistics for each company holding asset.
        /// </summary>
        internal List<AccountStatistics> AssetCompanyStats
        {
            get;
            private set;
        }

        /// <summary>
        /// Total statistics for each asset.
        /// </summary>
        internal List<AccountStatistics> AssetTotalStats
        {
            get;
            private set;
        }

        /// <summary>
        /// Any notes for the portfolio.
        /// </summary>
        private List<Note> PortfolioNotes
        {
            get;
            set;
        }

        /// <summary>
        /// Default empty constructor.
        /// </summary>
        public PortfolioStatistics()
        {
        }

        /// <summary>
        /// Constructor from a portfolio.
        /// </summary>
        public PortfolioStatistics(IPortfolio portfolio, PortfolioStatisticsSettings settings, IFileSystem fileSystem)
        {
            fDatabaseName = portfolio.DatabaseName(fileSystem);
            GenerateStatistics(portfolio, settings);
        }

        private void GenerateStatistics(IPortfolio portfolio, PortfolioStatisticsSettings settings)
        {
            Statistic[] bankAccountStatistics = settings.BankAccountGenerateOptions.GenerateFields.ToArray();

            PortfolioTotals = new List<AccountStatistics>();
            PortfolioTotals.AddRange(portfolio.GetStats(settings.DateToCalculate, Totals.Security, new TwoName(), bankAccountStatistics));
            PortfolioTotals.AddRange(portfolio.GetStats(settings.DateToCalculate, Totals.BankAccount, new TwoName(), bankAccountStatistics));
            PortfolioTotals.AddRange(portfolio.GetStats(settings.DateToCalculate, Totals.Asset, new TwoName(), bankAccountStatistics));
            PortfolioTotals.AddRange(portfolio.GetStats(settings.DateToCalculate, Totals.All, new TwoName(), bankAccountStatistics));

            GenerateSecurityStatistics(portfolio, settings);
            GenerateBankAccountStatistics(portfolio, settings);
            GenerateAssetStatistics(portfolio, settings);
            GenerateSectorStatistics(portfolio, settings);

            PortfolioNotes = portfolio.Notes.ToList();
        }

        private void GenerateSecurityStatistics(IPortfolio portfolio, PortfolioStatisticsSettings settings)
        {
            if (settings.SecurityGenerateOptions.ShouldGenerate)
            {
                Statistic[] securityData = settings.SecurityGenerateOptions.GenerateFields.ToArray();
                SecurityStats = portfolio.GetStats(settings.DateToCalculate, Account.Security, displayValueFunds: settings.DisplayValueFunds, displayTotals: false, securityData);
                SecurityCompanyStats = portfolio.GetStats(settings.DateToCalculate, Totals.SecurityCompany, settings.DisplayValueFunds, securityData);
                SecurityTotalStats = portfolio.GetStats(settings.DateToCalculate, Totals.Security, new TwoName(), securityData);
            }
        }

        private void GenerateBankAccountStatistics(IPortfolio portfolio, PortfolioStatisticsSettings settings)
        {
            if (settings.BankAccountGenerateOptions.ShouldGenerate)
            {
                Statistic[] bankAccountStatistics = settings.BankAccountGenerateOptions.GenerateFields.ToArray();
                BankAccountStats = portfolio.GetStats(settings.DateToCalculate, Account.BankAccount, settings.DisplayValueFunds, displayTotals: false, bankAccountStatistics);
                BankAccountCompanyStats = portfolio.GetStats(settings.DateToCalculate, Totals.BankAccountCompany, settings.DisplayValueFunds, bankAccountStatistics);
                BankAccountTotalStats = portfolio.GetStats(settings.DateToCalculate, Totals.BankAccount, new TwoName("Totals", ""), bankAccountStatistics);
            }
        }

        private void GenerateAssetStatistics(IPortfolio portfolio, PortfolioStatisticsSettings settings)
        {
            if (settings.AssetGenerateOptions.ShouldGenerate)
            {
                Statistic[] assetStatistics = settings.AssetGenerateOptions.GenerateFields.ToArray();
                AssetStats = portfolio.GetStats(settings.DateToCalculate, Account.Asset, settings.DisplayValueFunds, false, assetStatistics);

                AssetCompanyStats = portfolio.GetStats(settings.DateToCalculate, Totals.AssetCompany, settings.DisplayValueFunds, assetStatistics);
                AssetTotalStats = portfolio.GetStats(settings.DateToCalculate, Totals.Asset, new TwoName("Totals", ""), assetStatistics);
            }
        }

        private void GenerateSectorStatistics(IPortfolio portfolio, PortfolioStatisticsSettings settings)
        {
            if (settings.SectorGenerateOptions.ShouldGenerate)
            {
                SectorStats = new List<AccountStatistics>();
                Statistic[] sectorData = settings.SectorGenerateOptions.GenerateFields.ToArray();
                IReadOnlyList<string> sectorNames = portfolio.Sectors(Account.Security);
                foreach (string sectorName in sectorNames)
                {
                    SectorStats.AddRange(portfolio.GetStats(settings.DateToCalculate, Totals.Sector, new TwoName("Totals", sectorName), sectorData));

                    if (settings.GenerateBenchmarks)
                    {
                        SectorStats.AddRange(portfolio.GetStats(settings.DateToCalculate, Account.Benchmark, new TwoName("Benchmark", sectorName), sectorData));
                    }
                }
            }
        }

        /// <summary>
        /// Exports the statistics to a file.
        /// </summary>
        /// <param name="fileSystem">The file system interface to use.</param>
        /// <param name="filePath">The path exporting to.</param>
        /// <param name="exportType">The type of export.</param>
        /// <param name="settings">Various options for the export.</param>
        /// <param name="LogReporter">Returns information on success or failure.</param>
        public void ExportToFile(IFileSystem fileSystem, string filePath, DocumentType exportType, PortfolioStatisticsExportSettings settings, IReportLogger LogReporter)
        {
            try
            {
                ReportBuilder sb = ExportString(true, exportType, settings);

                using (Stream stream = fileSystem.FileStream.Create(filePath, FileMode.Create))
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    fileWriter.WriteLine(sb.ToString());
                }
            }
            catch (IOException exception)
            {
                _ = LogReporter.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.StatisticsPage, $"Error in exporting statistics page: {exception.Message}.");
                return;
            }

            _ = LogReporter.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.StatisticsPage, "Successfully exported statistics page.");
        }

        /// <summary>
        /// Creates the string with all stats.
        /// </summary>
        public ReportBuilder ExportString(bool includeHtmlHeaders, DocumentType exportType, PortfolioStatisticsExportSettings settings)
        {
            ReportBuilder reportBuilder = new ReportBuilder(exportType, new ReportSettings(settings.Colours, false, false));
            if (includeHtmlHeaders && exportType == DocumentType.Html)
            {
                _ = reportBuilder.WriteHeader($"Statement for funds as of {DateTime.Today.ToShortDateString()}")
                    .WriteTitle($"{fDatabaseName} - Statement on {DateTime.Today.ToShortDateString()}", DocumentElement.h1);
            }

            var totalFieldNames = PortfolioTotals.Select(data => data.Statistics).First().Select(stat => stat.StatType.ToString()).ToList();
            _ = reportBuilder.WriteTableFromEnumerable(
                totalFieldNames,
                PortfolioTotals.Select(data => data.Statistics),
                headerFirstColumn: false);

            WriteSection(reportBuilder, "Fund Data", settings.Spacing, settings.SecurityDisplayOptions, SecurityStats, SecurityCompanyStats, SecurityTotalStats);

            WriteSection(reportBuilder, "Bank Account Data", settings.Spacing, settings.BankAccountDisplayOptions, BankAccountStats, BankAccountCompanyStats, BankAccountTotalStats);

            WriteSection(reportBuilder, "Asset Data", settings.Spacing, settings.AssetDisplayOptions, AssetStats, AssetCompanyStats, AssetTotalStats);

            WriteSection(reportBuilder, "Analysis By Sector", settings.Spacing, settings.SectorDisplayOptions, SectorStats, null, null);

            _ = reportBuilder.WriteTitle("Portfolio Notes", DocumentElement.h2)
                .WriteTable(PortfolioNotes, headerFirstColumn: false);

            _ = reportBuilder.WriteFooter();

            return reportBuilder;
        }

        private static void WriteSection(
                ReportBuilder sb,
                string title,
                bool useSpacing,
                TableOptions<Statistic> displaySettings,
                IReadOnlyList<AccountStatistics> itemStats,
                IReadOnlyList<AccountStatistics> companyStats,
                IReadOnlyList<AccountStatistics> totalStats)
        {
            if (displaySettings.ShouldDisplay)
            {
                _ = sb.WriteTitle(title, DocumentElement.h2);
                List<AccountStatistics> collatedData = itemStats?.ToList() ?? new List<AccountStatistics>();

                if (companyStats != null)
                {
                    foreach (AccountStatistics companyStatistic in companyStats)
                    {
                        int number = collatedData.Count(datum => datum.NameData.Company.Equals(companyStatistic.NameData.Company, StringComparison.Ordinal));
                        if (number > 1)
                        {
                            collatedData.Add(companyStatistic);
                        }
                    }
                }

                collatedData.Sort(displaySettings.SortingField, displaySettings.SortingDirection);
                if (totalStats != null && totalStats.Count > 0)
                {
                    collatedData.AddRange(totalStats);
                }

                collatedData = AccountStatisticsHelpers.Restrict(collatedData, displaySettings.DisplayFields);

                AddSpacing(useSpacing, displaySettings.SortingField, ref collatedData, addAfterName: false);

                _ = sb.WriteTableFromEnumerable(
                    displaySettings.DisplayFields.Select(field => field.ToString()),
                    collatedData.Select(data => data.Statistics),
                    headerFirstColumn: true);
            }
        }

        /// <summary>
        /// Adds spacing into the table list if user desires.
        /// </summary>
        private static void AddSpacing(bool addSpacing, Statistic sortingField, ref List<AccountStatistics> dataList, bool addAfterName)
        {
            if (addSpacing)
            {
                if (sortingField == Statistic.Company || sortingField == Statistic.Name)
                {
                    int index = 0;
                    while (index < dataList.Count - 1)
                    {
                        if (!addAfterName && dataList[index].NameData.Company != dataList[index + 1].NameData.Company)
                        {
                            dataList.Insert(index + 1, new AccountStatistics());
                            index++;
                        }
                        else if (addAfterName && dataList[index].NameData.Name != dataList[index + 1].NameData.Name)
                        {
                            dataList.Insert(index + 1, new AccountStatistics());
                            index++;
                        }
                        index++;
                    }
                }
            }
        }
    }
}
