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
        private readonly PortfolioStatisticsSettings fDisplayOptions;

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
            fDisplayOptions = settings;
            GenerateStatistics(portfolio);
        }

        private void GenerateStatistics(IPortfolio portfolio)
        {
            PortfolioTotals.AddRange(portfolio.GetStats(Totals.Security, new TwoName(), AccountStatisticsHelpers.DefaultBankAccountStats()));
            PortfolioTotals.AddRange(portfolio.GetStats(Totals.BankAccount, new TwoName(), AccountStatisticsHelpers.DefaultBankAccountStats()));
            PortfolioTotals.AddRange(portfolio.GetStats(Totals.All, new TwoName(), AccountStatisticsHelpers.DefaultBankAccountStats()));

            Statistic[] securityData = fDisplayOptions.SecurityDisplayOptions.DisplayFields.ToArray();

            IndividualSecurityStats = portfolio.GetStats(Account.Security, displayValueFunds: fDisplayOptions.DisplayValueFunds, displayTotals: false, securityData);

            CompanyTotalsStats = portfolio.GetStats(Totals.SecurityCompany, fDisplayOptions.DisplayValueFunds, securityData);
            PortfolioSecurityStats = portfolio.GetStats(Totals.Security, new TwoName(), securityData);

            BankAccountStats = portfolio.GetStats(Account.BankAccount, fDisplayOptions.DisplayValueFunds, false);

            Statistic[] bankAccountData = fDisplayOptions.BankAccountDisplayOptions.DisplayFields.ToArray();
            BankAccountCompanyStats = portfolio.GetStats(Totals.BankAccountCompany, fDisplayOptions.DisplayValueFunds, bankAccountData);
            BankAccountTotalStats = portfolio.GetStats(Totals.BankAccount, new TwoName("Totals", ""), bankAccountData);

            Statistic[] sectorData = fDisplayOptions.SectorDisplayOptions.DisplayFields.ToArray();
            IReadOnlyList<string> sectorNames = portfolio.Sectors(Account.Security);
            foreach (string sectorName in sectorNames)
            {
                SectorStats.AddRange(portfolio.GetStats(Totals.Sector, new TwoName("Totals", sectorName), sectorData));

                if (fDisplayOptions.IncludeBenchmarks)
                {
                    SectorStats.AddRange(portfolio.GetStats(Account.Benchmark, new TwoName("Benchmark", sectorName), statisticsToDisplay: sectorData));
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

            TableWriting.WriteTableFromEnumerable(sb, exportType, fDisplayOptions.BankAccountDisplayOptions.DisplayFieldNames(), PortfolioTotals.Select(data => data.Statistics), headerFirstColumn: false);

            if (fDisplayOptions.SecurityDisplayOptions.ShouldDisplay)
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

                securityDataToWrite.Sort(fDisplayOptions.SecurityDisplayOptions.SortingField, fDisplayOptions.SectorDisplayOptions.SortingDirection);
                securityDataToWrite.AddRange(PortfolioSecurityStats);

                SpacingAdd(settings.Spacing, fDisplayOptions.SecurityDisplayOptions.SortingField, ref securityDataToWrite);

                TableWriting.WriteTableFromEnumerable(sb, exportType, fDisplayOptions.SecurityDisplayOptions.DisplayFieldNames(), securityDataToWrite.Select(data => data.Statistics), headerFirstColumn: true);
            }

            if (fDisplayOptions.BankAccountDisplayOptions.ShouldDisplay)
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

                bankAccountDataToWrite.Sort(fDisplayOptions.BankAccountDisplayOptions.SortingField, fDisplayOptions.BankAccountDisplayOptions.SortingDirection);
                bankAccountDataToWrite.AddRange(BankAccountTotalStats);

                SpacingAdd(settings.Spacing, fDisplayOptions.BankAccountDisplayOptions.SortingField, ref bankAccountDataToWrite);

                TableWriting.WriteTableFromEnumerable(sb, exportType, fDisplayOptions.BankAccountDisplayOptions.DisplayFieldNames(), bankAccountDataToWrite.Select(data => data.Statistics), headerFirstColumn: true);
            }

            if (fDisplayOptions.SectorDisplayOptions.ShouldDisplay)
            {
                TextWriting.WriteTitle(sb, exportType, "Analysis By Sector", HtmlTag.h2);
                List<AccountStatistics> sectorDataToWrite = SectorStats;

                sectorDataToWrite.Sort(fDisplayOptions.SectorDisplayOptions.SortingField, fDisplayOptions.SectorDisplayOptions.SortingDirection);

                SectorSpacingAdd(settings.Spacing, fDisplayOptions.SectorDisplayOptions.SortingField, ref sectorDataToWrite);

                TableWriting.WriteTableFromEnumerable(sb, exportType, fDisplayOptions.SectorDisplayOptions.DisplayFieldNames(), sectorDataToWrite.Select(data => data.Statistics), headerFirstColumn: true);
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
        private static void SpacingAdd(bool addSpacing, Statistic sortingField, ref List<AccountStatistics> dataList)
        {
            if (addSpacing)
            {
                if (sortingField == Statistic.Company || sortingField == Statistic.Name)
                {
                    int index = 0;
                    while (index < dataList.Count - 1)
                    {
                        if (dataList[index].NameData.Company != dataList[index + 1].NameData.Company)
                        {
                            dataList.Insert(index + 1, new AccountStatistics());
                            index++;
                        }
                        index++;
                    }
                }
            }
        }

        /// <summary>
        /// Adds spacing into the table list if user desires.
        /// </summary>
        private static void SectorSpacingAdd(bool addSpacing, Statistic sortingField, ref List<AccountStatistics> dataList)
        {
            if (addSpacing)
            {
                if (sortingField == Statistic.Company || sortingField == Statistic.Name)
                {
                    int index = 0;
                    while (index < dataList.Count - 1)
                    {
                        if (dataList[index].NameData.Name != dataList[index + 1].NameData.Name)
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
