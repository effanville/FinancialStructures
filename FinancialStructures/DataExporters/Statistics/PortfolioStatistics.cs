using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using Common.Structure.FileAccess;
using Common.Structure.Reporting;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.NamingStructures;
using FinancialStructures.Statistics;

namespace FinancialStructures.DataExporters.Statistics
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
        } = new List<AccountStatistics>();

        /// <summary>
        /// List of statistics of each company performance
        /// </summary>
        public List<AccountStatistics> CompanyTotalsStats
        {
            get;
            set;
        } = new List<AccountStatistics>();

        /// <summary>
        /// List of statistics of each company performance
        /// </summary>
        public List<AccountStatistics> PortfolioSecurityStats
        {
            get;
            set;
        } = new List<AccountStatistics>();

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
        } = new List<AccountStatistics>();

        /// <summary>
        /// Statistics for each company holding bank accounts.
        /// </summary>
        public List<AccountStatistics> BankAccountCompanyStats
        {
            get;
            set;
        } = new List<AccountStatistics>();

        /// <summary>
        /// Total statistics for each BankAccount.
        /// </summary>
        public List<AccountStatistics> BankAccountTotalStats
        {
            get;
            set;
        } = new List<AccountStatistics>();

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
                using (Stream stream = fileSystem.FileStream.Create(filePath, FileMode.Create))
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    if (exportType == ExportType.Html)
                    {
                        fileWriter.CreateHTMLHeader($"Statement for funds as of {DateTime.Today.ToShortDateString()}", settings.Colours);
                        fileWriter.WriteLine($"<h1>{fDatabaseName} - Statement on {DateTime.Today.ToShortDateString()}</h1>");
                    }

                    fileWriter.WriteTableFromEnumerable(exportType, fDisplayOptions.BankAccountDisplayOptions.DisplayFieldNames(), PortfolioTotals.Select(data => data.Statistics), false);

                    if (fDisplayOptions.SecurityDisplayOptions.ShouldDisplay)
                    {
                        fileWriter.WriteTitle(exportType, "Funds Data", HtmlTag.h2);
                        List<AccountStatistics> securityDataToWrite = IndividualSecurityStats;

                        foreach (AccountStatistics companyStatistic in CompanyTotalsStats)
                        {
                            int number = securityDataToWrite.Count(datum => datum.NameData.Company.Equals(companyStatistic.NameData.Company, StringComparison.Ordinal));
                            if (number > 1)
                            {
                                securityDataToWrite.Add(companyStatistic);
                            }
                        }

                        securityDataToWrite.Sort(fDisplayOptions.SecurityDisplayOptions);
                        securityDataToWrite.AddRange(PortfolioSecurityStats);

                        SpacingAdd(settings.Spacing, fDisplayOptions.SecurityDisplayOptions.SortingField, ref securityDataToWrite);

                        fileWriter.WriteTableFromEnumerable(exportType, fDisplayOptions.SecurityDisplayOptions.DisplayFieldNames(), securityDataToWrite.Select(data => data.Statistics), true);
                    }

                    if (fDisplayOptions.BankAccountDisplayOptions.ShouldDisplay)
                    {
                        fileWriter.WriteTitle(exportType, "Bank Accounts Data", HtmlTag.h2);
                        List<AccountStatistics> bankAccountDataToWrite = BankAccountStats;

                        foreach (AccountStatistics companyStatistic in BankAccountCompanyStats)
                        {
                            int number = bankAccountDataToWrite.Count(datum => datum.NameData.Company.Equals(companyStatistic.NameData.Company, StringComparison.Ordinal));
                            if (number > 1)
                            {
                                bankAccountDataToWrite.Add(companyStatistic);
                            }
                        }

                        bankAccountDataToWrite.Sort(fDisplayOptions.BankAccountDisplayOptions);
                        bankAccountDataToWrite.AddRange(BankAccountTotalStats);

                        SpacingAdd(settings.Spacing, fDisplayOptions.BankAccountDisplayOptions.SortingField, ref bankAccountDataToWrite);

                        fileWriter.WriteTableFromEnumerable(exportType, fDisplayOptions.BankAccountDisplayOptions.DisplayFieldNames(), bankAccountDataToWrite.Select(data => data.Statistics), true);
                    }

                    if (fDisplayOptions.SectorDisplayOptions.ShouldDisplay)
                    {
                        fileWriter.WriteTitle(exportType, "Analysis By Sector", HtmlTag.h2);
                        List<AccountStatistics> sectorDataToWrite = SectorStats;

                        sectorDataToWrite.Sort(fDisplayOptions.SectorDisplayOptions);

                        SectorSpacingAdd(settings.Spacing, fDisplayOptions.SectorDisplayOptions.SortingField, ref sectorDataToWrite);

                        fileWriter.WriteTableFromEnumerable(exportType, fDisplayOptions.SectorDisplayOptions.DisplayFieldNames(), sectorDataToWrite.Select(data => data.Statistics), true);
                    }

                    if (exportType == ExportType.Html)
                    {
                        fileWriter.CreateHTMLFooter();
                    }

                    fileWriter.Close();
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
