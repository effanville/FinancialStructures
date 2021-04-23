using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.DataExporters.ExportOptions;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;
using FinancialStructures.Statistics;
using StructureCommon.FileAccess;
using StructureCommon.Reporting;

namespace FinancialStructures.DataExporters
{
    /// <summary>
    /// Container for statistics on a portfolio.
    /// </summary>
    public class PortfolioStatistics
    {
        private readonly string fDatabaseName;
        private readonly UserDisplayOptions fDisplayOptions;

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
        public PortfolioStatistics(IPortfolio portfolio, UserDisplayOptions displayOptions)
        {
            fDatabaseName = portfolio.DatabaseName;
            fDisplayOptions = displayOptions;
            GenerateStatistics(portfolio);
        }

        private void GenerateStatistics(IPortfolio portfolio)
        {
            PortfolioTotals.AddRange(portfolio.GetStats(Totals.Security, new TwoName(), AccountStatisticsHelpers.DefaultBankAccountStats()));
            PortfolioTotals.AddRange(portfolio.GetStats(Totals.BankAccount, new TwoName(), AccountStatisticsHelpers.DefaultBankAccountStats()));
            PortfolioTotals.AddRange(portfolio.GetStats(Totals.All, new TwoName(), AccountStatisticsHelpers.DefaultBankAccountStats()));

            var securityData = fDisplayOptions.SecurityDisplayOptions.DisplayFields.ToArray();

            IndividualSecurityStats = portfolio.GetStats(Account.Security, displayValueFunds: fDisplayOptions.DisplayValueFunds, displayTotals: false, securityData);

            CompanyTotalsStats = portfolio.GetStats(Totals.SecurityCompany, fDisplayOptions.DisplayValueFunds, securityData);
            PortfolioSecurityStats = portfolio.GetStats(Totals.Security, new TwoName(), securityData);

            BankAccountStats = portfolio.GetStats(Account.BankAccount, fDisplayOptions.DisplayValueFunds, false);

            var bankAccountData = fDisplayOptions.BankAccountDisplayOptions.DisplayFields.ToArray();
            BankAccountCompanyStats = portfolio.GetStats(Totals.BankAccount, fDisplayOptions.DisplayValueFunds, bankAccountData);
            BankAccountTotalStats = portfolio.GetStats(Totals.BankAccount, new TwoName("Totals", ""), bankAccountData);

            var sectorData = fDisplayOptions.SectorDisplayOptions.DisplayFields.ToArray();
            List<string> sectorNames = portfolio.GetSecuritiesSectors();
            foreach (string sectorName in sectorNames)
            {
                SectorStats.AddRange(portfolio.GetStats(Totals.Sector, new TwoName("Totals", sectorName), sectorData));
                SectorStats.AddRange(portfolio.GetStats(Account.Benchmark, new TwoName("Benchmark", sectorName), statisticsToDisplay: sectorData));
            }
        }

        /// <summary>
        /// Exports the statistics to a file.
        /// </summary>
        /// <param name="filePath">The path exporting to.</param>
        /// <param name="exportType">The type of export.</param>
        /// <param name="options">Various options the user has specified.</param>
        /// <param name="LogReporter">Returns information on success or failure.</param>
        public void ExportToFile(string filePath, ExportType exportType, UserDisplayOptions options, IReportLogger LogReporter)
        {
            try
            {
                StreamWriter fileWriter = new StreamWriter(filePath);
                if (exportType == ExportType.Html)
                {
                    fileWriter.CreateHTMLHeader($"Statement for funds as of {DateTime.Today.ToShortDateString()}", fDisplayOptions.Colours);
                    fileWriter.WriteLine($"<h1>{fDatabaseName} - Statement on {DateTime.Today.ToShortDateString()}</h1>");
                }

                fileWriter.WriteTableFromEnumerable(exportType, options.BankAccountDisplayOptions.DisplayFieldNames(), PortfolioTotals.Select(data => data.Statistics), false);

                if (options.SecurityDisplayOptions.ShouldDisplay)
                {
                    fileWriter.WriteTitle(exportType, "Funds Data", HtmlTag.h2);
                    List<AccountStatistics> securityDataToWrite = IndividualSecurityStats;

                    foreach (var companyStatistic in CompanyTotalsStats)
                    {
                        int number = securityDataToWrite.Count(datum => datum.NameData.Company.Equals(companyStatistic.NameData.Company));
                        if (number > 1)
                        {
                            securityDataToWrite.Add(companyStatistic);
                        }
                    }

                    securityDataToWrite.Sort(options.SecurityDisplayOptions);
                    securityDataToWrite.AddRange(PortfolioSecurityStats);

                    SpacingAdd(options.Spacing, options.SecurityDisplayOptions.SortingField, ref securityDataToWrite);

                    fileWriter.WriteTableFromEnumerable(exportType, options.SecurityDisplayOptions.DisplayFieldNames(), securityDataToWrite.Select(data => data.Statistics), true);
                }

                if (options.BankAccountDisplayOptions.ShouldDisplay)
                {
                    fileWriter.WriteTitle(exportType, "Bank Accounts Data", HtmlTag.h2);
                    List<AccountStatistics> bankAccountDataToWrite = BankAccountStats;

                    foreach (var companyStatistic in BankAccountCompanyStats)
                    {
                        int number = bankAccountDataToWrite.Count(datum => datum.NameData.Company.Equals(companyStatistic.NameData.Company));
                        if (number > 1)
                        {
                            bankAccountDataToWrite.Add(companyStatistic);
                        }
                    }

                    bankAccountDataToWrite.Sort(options.BankAccountDisplayOptions);
                    bankAccountDataToWrite.AddRange(BankAccountTotalStats);

                    SpacingAdd(options.Spacing, options.BankAccountDisplayOptions.SortingField, ref bankAccountDataToWrite);

                    fileWriter.WriteTableFromEnumerable(exportType, options.BankAccountDisplayOptions.DisplayFieldNames(), bankAccountDataToWrite.Select(data => data.Statistics), true);
                }

                if (options.SectorDisplayOptions.ShouldDisplay)
                {
                    fileWriter.WriteTitle(exportType, "Analysis By Sector", HtmlTag.h2);
                    List<AccountStatistics> sectorDataToWrite = SectorStats;

                    sectorDataToWrite.Sort(options.SectorDisplayOptions);

                    SpacingAdd(options.Spacing, options.SectorDisplayOptions.SortingField, ref sectorDataToWrite);

                    fileWriter.WriteTableFromEnumerable(exportType, options.SectorDisplayOptions.DisplayFieldNames(), sectorDataToWrite.Select(data => data.Statistics), true);
                }

                if (exportType == ExportType.Html)
                {
                    fileWriter.CreateHTMLFooter();
                }

                fileWriter.Close();
            }
            catch (IOException exception)
            {
                _ = LogReporter.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.StatisticsPage, $"Error in exporting statistics page: {exception.Message}.");
                return;
            }

            _ = LogReporter.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.StatisticsPage, "Successfully exported statistics page.");
        }

        /// <summary>
        /// Adds spacing into the table list if user desires.
        /// </summary>
        private void SpacingAdd(bool addSpacing, Statistic sortingField, ref List<AccountStatistics> dataList)
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
    }
}
