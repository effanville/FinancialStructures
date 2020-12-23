using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FinancialStructures.Statistics;
using StructureCommon.FileAccess;
using StructureCommon.Reporting;

namespace FinancialStructures.StatsMakers
{
    /// <summary>
    /// Container for statistics on a portfolio.
    /// </summary>
    public class PortfolioStatistics
    {
        private readonly string fDatabaseName;
        private readonly UserOptions fDisplayOptions;

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
        public PortfolioStatistics(IPortfolio portfolio, UserOptions displayOptions)
        {
            fDatabaseName = portfolio.DatabaseName;
            fDisplayOptions = displayOptions;
            GenerateStatistics(portfolio);
        }

        private void GenerateStatistics(IPortfolio portfolio)
        {
            PortfolioTotals.AddRange(portfolio.GetStats(Totals.Security, displayValueFunds: fDisplayOptions.DisplayValueFunds, AccountStatisticsHelpers.DefaultBankAccountStats()));
            PortfolioTotals.AddRange(portfolio.GetStats(Totals.BankAccount, displayValueFunds: fDisplayOptions.DisplayValueFunds, AccountStatisticsHelpers.DefaultBankAccountStats()));
            PortfolioTotals.AddRange(portfolio.GetStats(Totals.All, displayValueFunds: fDisplayOptions.DisplayValueFunds, AccountStatisticsHelpers.DefaultBankAccountStats()));

            IndividualSecurityStats = portfolio.GetStats(Account.Security, displayValueFunds: fDisplayOptions.DisplayValueFunds, displayTotals: false, fDisplayOptions.SecurityDataToExport.ToArray());

            CompanyTotalsStats = portfolio.GetStats(Totals.SecurityCompany, fDisplayOptions.DisplayValueFunds, fDisplayOptions.SecurityDataToExport.ToArray());
            PortfolioSecurityStats = portfolio.GetStats(Totals.Security, fDisplayOptions.DisplayValueFunds, fDisplayOptions.SecurityDataToExport.ToArray());

            BankAccountStats = portfolio.GetStats(Account.BankAccount, fDisplayOptions.DisplayValueFunds, false);

            BankAccountCompanyStats = portfolio.GetStats(Totals.BankAccount, fDisplayOptions.DisplayValueFunds, fDisplayOptions.BankAccDataToExport.ToArray());
            BankAccountTotalStats = portfolio.GetStats(Totals.BankAccount, new TwoName("Totals", ""));

            List<string> sectorNames = portfolio.GetSecuritiesSectors();
            foreach (string sectorName in sectorNames)
            {
                SectorStats.AddRange(portfolio.GetStats(Totals.Sector, new TwoName("Totals", sectorName), fDisplayOptions.SectorDataToExport.ToArray()));
                SectorStats.AddRange(portfolio.GetStats(Account.Benchmark, new TwoName("Benchmark", sectorName), statisticsToDisplay: fDisplayOptions.SectorDataToExport.ToArray()));
            }
        }

        /// <summary>
        /// Exports the statistics to a file.
        /// </summary>
        /// <param name="filePath">The path exporting to.</param>
        /// <param name="exportType">The type of export.</param>
        /// <param name="options">Various options the user has specified.</param>
        /// <param name="LogReporter">Returns information on success or failure.</param>
        public void ExportToFile(string filePath, ExportType exportType, UserOptions options, IReportLogger LogReporter)
        {
            try
            {
                StreamWriter fileWriter = new StreamWriter(filePath);
                if (exportType == ExportType.Html)
                {
                    fileWriter.CreateHTMLHeader($"Statement for funds as of {DateTime.Today.ToShortDateString()}", fDisplayOptions.Colours);
                    fileWriter.WriteLine($"<h1>{fDatabaseName} - Statement on {DateTime.Today.ToShortDateString()}</h1>");
                }

                fileWriter.WriteTableFromEnumerable(exportType, options.BankAccDataToExport.Select(data => data.ToString()), PortfolioTotals.Select(data => data.Statistics), false);

                if (options.ShowSecurites)
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

                    securityDataToWrite.Sort(options.SecuritySortingField, options.SecuritySortDirection);

                    securityDataToWrite.AddRange(PortfolioSecurityStats);
                    if (options.Spacing)
                    {
                        if (options.SecuritySortingField == Statistic.Company || options.SecuritySortingField == Statistic.Name)
                        {
                            int index = 0;
                            while (index < securityDataToWrite.Count - 1)
                            {
                                if (securityDataToWrite[index].NameData.Company != securityDataToWrite[index + 1].NameData.Company)
                                {
                                    securityDataToWrite.Insert(index + 1, new AccountStatistics());
                                    index++;
                                }
                                index++;
                            }
                        }
                    }

                    // spacing causes null rows which go wrong.
                    fileWriter.WriteTableFromEnumerable(exportType, options.SecurityDataToExport.Select(data => data.ToString()), securityDataToWrite.Select(data => data.Statistics), true);
                }

                if (options.ShowBankAccounts)
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

                    bankAccountDataToWrite.Sort(options.BankAccountSortingField, options.BankSortDirection);
                    bankAccountDataToWrite.AddRange(BankAccountTotalStats);

                    if (options.Spacing)
                    {
                        if (options.BankAccountSortingField == Statistic.Company || options.BankAccountSortingField == Statistic.Name)
                        {
                            int index = 0;
                            while (index < bankAccountDataToWrite.Count - 1)
                            {
                                if (bankAccountDataToWrite[index].NameData.Company != bankAccountDataToWrite[index + 1].NameData.Company)
                                {
                                    bankAccountDataToWrite.Insert(index + 1, new AccountStatistics());
                                    index++;
                                }
                                index++;
                            }
                        }
                    }

                    fileWriter.WriteTableFromEnumerable(exportType, options.BankAccDataToExport.Select(data => data.ToString()), bankAccountDataToWrite.Select(data => data.Statistics), true);
                }

                if (options.ShowSectors)
                {
                    fileWriter.WriteTitle(exportType, "Analysis By Sector", HtmlTag.h2);
                    List<AccountStatistics> sectorDataToWrite = SectorStats;

                    sectorDataToWrite.Sort(options.SectorSortingField, options.SectorSortDirection);
                    if (options.Spacing)
                    {
                        if (options.SectorSortingField == Statistic.Company || options.SectorSortingField == Statistic.Name)
                        {
                            int i = 0;
                            while (i < sectorDataToWrite.Count - 1)
                            {
                                if (sectorDataToWrite[i].NameData.Name != sectorDataToWrite[i + 1].NameData.Name)
                                {
                                    sectorDataToWrite.Insert(i + 1, new AccountStatistics());
                                    i++;
                                }
                                i++;
                            }
                        }
                    }

                    fileWriter.WriteTableFromEnumerable(exportType, options.SectorDataToExport.Select(data => data.ToString()), sectorDataToWrite.Select(data => data.Statistics), true);
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
    }
}
