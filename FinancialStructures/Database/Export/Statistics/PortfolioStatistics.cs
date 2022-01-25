using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using Common.Structure.FileAccess;
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
        private readonly PortfolioStatisticsSettings fGenerationSettings;

        /// <summary>
        /// Totals of different types held in portfolio.
        /// </summary>
        public List<AccountStatistics> PortfolioTotals
        {
            get;
            set;
        } = new List<AccountStatistics>();

        /// <summary>
        /// A list of all securities and their statistics.
        /// </summary>
        public List<AccountStatistics> IndividualSecurityStats
        {
            get;
            set;
        }

        /// <summary>
        /// List of statistics of each company performance
        /// </summary>
        public List<AccountStatistics> CompanyTotalsStats
        {
            get;
            set;
        }

        /// <summary>
        /// List of statistics of each company performance
        /// </summary>
        public List<AccountStatistics> PortfolioSecurityStats
        {
            get;
            set;
        }

        /// <summary>
        /// Each specified sectors performance.
        /// </summary>
        public List<AccountStatistics> SectorStats
        {
            get;
            set;
        } = new List<AccountStatistics>();

        /// <summary>
        /// Value held in each bank account.
        /// </summary>
        public List<AccountStatistics> BankAccountStats
        {
            get;
            set;
        }

        /// <summary>
        /// Statistics for each company holding bank accounts.
        /// </summary>
        public List<AccountStatistics> BankAccountCompanyStats
        {
            get;
            set;
        }

        /// <summary>
        /// Total statistics for each BankAccount.
        /// </summary>
        public List<AccountStatistics> BankAccountTotalStats
        {
            get;
            set;
        }

        /// <summary>
        /// Value held in each asset.
        /// </summary>
        public List<AccountStatistics> AssetStats
        {
            get;
            set;
        }

        /// <summary>
        /// Statistics for each company holding asset.
        /// </summary>
        public List<AccountStatistics> AssetCompanyStats
        {
            get;
            set;
        }

        /// <summary>
        /// Total statistics for each asset.
        /// </summary>
        public List<AccountStatistics> AssetTotalStats
        {
            get;
            set;
        }

        /// <summary>
        /// Any notes for the portfolio.
        /// </summary>
        public List<Note> PortfolioNotes
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
            fGenerationSettings = settings;
            GenerateStatistics(portfolio);
        }

        private void GenerateStatistics(IPortfolio portfolio)
        {
            PortfolioTotals.AddRange(portfolio.GetStats(fGenerationSettings.DateToCalculate, Totals.Security, new TwoName(), AccountStatisticsHelpers.DefaultBankAccountStats()));
            PortfolioTotals.AddRange(portfolio.GetStats(fGenerationSettings.DateToCalculate, Totals.BankAccount, new TwoName(), AccountStatisticsHelpers.DefaultBankAccountStats()));
            PortfolioTotals.AddRange(portfolio.GetStats(fGenerationSettings.DateToCalculate, Totals.Asset, new TwoName(), AccountStatisticsHelpers.DefaultBankAccountStats()));
            PortfolioTotals.AddRange(portfolio.GetStats(fGenerationSettings.DateToCalculate, Totals.All, new TwoName(), AccountStatisticsHelpers.DefaultBankAccountStats()));

            Statistic[] securityData = fGenerationSettings.SecurityDisplayOptions.DisplayFields.ToArray();

            IndividualSecurityStats = portfolio.GetStats(fGenerationSettings.DateToCalculate, Account.Security, displayValueFunds: fGenerationSettings.DisplayValueFunds, displayTotals: false, statisticsToDisplay: securityData);

            CompanyTotalsStats = portfolio.GetStats(fGenerationSettings.DateToCalculate, Totals.SecurityCompany, fGenerationSettings.DisplayValueFunds, securityData);
            PortfolioSecurityStats = portfolio.GetStats(fGenerationSettings.DateToCalculate, Totals.Security, new TwoName(), securityData);

            BankAccountStats = portfolio.GetStats(fGenerationSettings.DateToCalculate, Account.BankAccount, fGenerationSettings.DisplayValueFunds, false);

            Statistic[] bankAccountData = fGenerationSettings.BankAccountDisplayOptions.DisplayFields.ToArray();
            BankAccountCompanyStats = portfolio.GetStats(fGenerationSettings.DateToCalculate, Totals.BankAccountCompany, fGenerationSettings.DisplayValueFunds, bankAccountData);
            BankAccountTotalStats = portfolio.GetStats(fGenerationSettings.DateToCalculate, Totals.BankAccount, new TwoName("Totals", ""), bankAccountData);

            AssetStats = portfolio.GetStats(fGenerationSettings.DateToCalculate, Account.Asset, fGenerationSettings.DisplayValueFunds, false);

            Statistic[] assetStatistics = fGenerationSettings.AssetDisplayOptions.DisplayFields.ToArray();
            AssetCompanyStats = portfolio.GetStats(fGenerationSettings.DateToCalculate, Totals.AssetCompany, fGenerationSettings.DisplayValueFunds, assetStatistics);
            AssetTotalStats = portfolio.GetStats(fGenerationSettings.DateToCalculate, Totals.Asset, new TwoName("Totals", ""), assetStatistics);

            Statistic[] sectorData = fGenerationSettings.SectorDisplayOptions.DisplayFields.ToArray();
            IReadOnlyList<string> sectorNames = portfolio.Sectors(Account.Security);
            foreach (string sectorName in sectorNames)
            {
                SectorStats.AddRange(portfolio.GetStats(fGenerationSettings.DateToCalculate, Totals.Sector, new TwoName("Totals", sectorName), sectorData));

                if (fGenerationSettings.IncludeBenchmarks)
                {
                    SectorStats.AddRange(portfolio.GetStats(fGenerationSettings.DateToCalculate, Account.Benchmark, new TwoName("Benchmark", sectorName), statisticsToDisplay: sectorData));
                }
            }

            PortfolioNotes = portfolio.Notes.ToList();
        }

        /// <summary>
        /// Exports the statistics to a file.
        /// </summary>
        /// <param name="fileSystem">The file system interface to use.</param>
        /// <param name="filePath">The path exporting to.</param>
        /// <param name="exportType">The type of export.</param>
        /// <param name="settings">Various options for the export.</param>
        /// <param name="LogReporter">Returns information on success or failure.</param>
        public void ExportToFile(IFileSystem fileSystem, string filePath, ExportType exportType, PortfolioStatisticsExportSettings settings, IReportLogger LogReporter)
        {
            try
            {
                StringBuilder sb = ExportString(true, exportType, settings);

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
        public StringBuilder ExportString(bool includeHtmlHeaders, ExportType exportType, PortfolioStatisticsExportSettings settings)
        {
            StringBuilder sb = new StringBuilder();
            if (includeHtmlHeaders && exportType == ExportType.Html)
            {
                TextWriting.CreateHTMLHeader(sb, $"Statement for funds as of {DateTime.Today.ToShortDateString()}", settings.Colours);
                _ = sb.AppendLine($"<h1>{fDatabaseName} - Statement on {DateTime.Today.ToShortDateString()}</h1>");
            }

            TableWriting.WriteTableFromEnumerable(
                sb,
                exportType,
                fGenerationSettings.BankAccountDisplayOptions.DisplayFieldNames(),
                PortfolioTotals.Select(data => data.Statistics),
                headerFirstColumn: false);

            if (fGenerationSettings.SecurityDisplayOptions.ShouldDisplay)
            {
                TextWriting.WriteTitle(sb, exportType, "Funds Data", HtmlTag.h2);
                List<AccountStatistics> securityDataToWrite = IndividualSecurityStats;

                foreach (AccountStatistics companyStatistic in CompanyTotalsStats)
                {
                    int number = securityDataToWrite.Count(datum => datum.NameData.Company.Equals(companyStatistic.NameData.Company, StringComparison.Ordinal));
                    if (number > 1)
                    {
                        securityDataToWrite.Add(companyStatistic);
                    }
                }

                securityDataToWrite.Sort(fGenerationSettings.SecurityDisplayOptions.SortingField, fGenerationSettings.SecurityDisplayOptions.SortingDirection);
                securityDataToWrite.AddRange(PortfolioSecurityStats);

                AddSpacing(settings.Spacing, fGenerationSettings.SecurityDisplayOptions.SortingField, ref securityDataToWrite, addAfterName: false);

                TableWriting.WriteTableFromEnumerable(
                    sb,
                    exportType,
                    fGenerationSettings.SecurityDisplayOptions.DisplayFieldNames(),
                    securityDataToWrite.Select(data => data.Statistics),
                    headerFirstColumn: true);
            }

            if (fGenerationSettings.BankAccountDisplayOptions.ShouldDisplay)
            {
                TextWriting.WriteTitle(sb, exportType, "Bank Accounts Data", HtmlTag.h2);
                List<AccountStatistics> bankAccountDataToWrite = BankAccountStats;

                foreach (AccountStatistics companyStatistic in BankAccountCompanyStats)
                {
                    int number = bankAccountDataToWrite.Count(datum => datum.NameData.Company.Equals(companyStatistic.NameData.Company, StringComparison.Ordinal));
                    if (number > 1)
                    {
                        bankAccountDataToWrite.Add(companyStatistic);
                    }
                }

                bankAccountDataToWrite.Sort(fGenerationSettings.BankAccountDisplayOptions.SortingField, fGenerationSettings.BankAccountDisplayOptions.SortingDirection);
                bankAccountDataToWrite.AddRange(BankAccountTotalStats);

                AddSpacing(settings.Spacing, fGenerationSettings.BankAccountDisplayOptions.SortingField, ref bankAccountDataToWrite, addAfterName: false);

                TableWriting.WriteTableFromEnumerable(
                    sb,
                    exportType,
                    fGenerationSettings.BankAccountDisplayOptions.DisplayFieldNames(),
                    bankAccountDataToWrite.Select(data => data.Statistics),
                    headerFirstColumn: true);
            }

            if (fGenerationSettings.AssetDisplayOptions.ShouldDisplay)
            {
                TextWriting.WriteTitle(sb, exportType, "Asset Data", HtmlTag.h2);
                List<AccountStatistics> assetDataToWrite = AssetStats;

                foreach (AccountStatistics companyStatistic in AssetCompanyStats)
                {
                    int number = assetDataToWrite.Count(datum => datum.NameData.Company.Equals(companyStatistic.NameData.Company, StringComparison.Ordinal));
                    if (number > 1)
                    {
                        assetDataToWrite.Add(companyStatistic);
                    }
                }

                assetDataToWrite.Sort(fGenerationSettings.AssetDisplayOptions.SortingField, fGenerationSettings.AssetDisplayOptions.SortingDirection);
                assetDataToWrite.AddRange(AssetTotalStats);

                AddSpacing(settings.Spacing, fGenerationSettings.AssetDisplayOptions.SortingField, ref assetDataToWrite, addAfterName: false);

                TableWriting.WriteTableFromEnumerable(
                    sb,
                    exportType,
                    fGenerationSettings.AssetDisplayOptions.DisplayFieldNames(),
                    assetDataToWrite.Select(data => data.Statistics),
                    headerFirstColumn: true);
            }

            if (fGenerationSettings.SectorDisplayOptions.ShouldDisplay)
            {
                TextWriting.WriteTitle(sb, exportType, "Analysis By Sector", HtmlTag.h2);
                List<AccountStatistics> sectorDataToWrite = SectorStats;

                sectorDataToWrite.Sort(fGenerationSettings.SectorDisplayOptions.SortingField, fGenerationSettings.SectorDisplayOptions.SortingDirection);

                AddSpacing(settings.Spacing, fGenerationSettings.SectorDisplayOptions.SortingField, ref sectorDataToWrite, addAfterName: true);

                TableWriting.WriteTableFromEnumerable(
                    sb,
                    exportType,
                    fGenerationSettings.SectorDisplayOptions.DisplayFieldNames(),
                    sectorDataToWrite.Select(data => data.Statistics),
                    headerFirstColumn: true);
            }

            TextWriting.WriteTitle(sb, exportType, "Portfolio Notes", HtmlTag.h2);
            TableWriting.WriteTable(sb, exportType, PortfolioNotes, headerFirstColumn: false);

            if (includeHtmlHeaders && exportType == ExportType.Html)
            {
                TextWriting.CreateHTMLFooter(sb);
            }

            return sb;
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
