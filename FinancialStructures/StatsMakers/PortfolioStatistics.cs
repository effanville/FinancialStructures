using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FinancialStructures.StatisticStructures;
using StructureCommon.Extensions;
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

        /// <summary>
        /// Totals of different types held in portfolio.
        /// </summary>
        public List<DayValue_Named> Totals
        {
            get;
            set;
        } = new List<DayValue_Named>();

        /// <summary>
        /// A list of all securities and their statistics.
        /// </summary>
        public List<SecurityStatistics> IndividualSecurityStats
        {
            get;
            set;
        } = new List<SecurityStatistics>();

        /// <summary>
        /// List of statistics of each company performance
        /// </summary>
        public List<SecurityStatistics> CompanyTotalsStats
        {
            get;
            set;
        } = new List<SecurityStatistics>();

        /// <summary>
        /// List of statistics of each company performance
        /// </summary>
        public SecurityStatistics PortfolioSecurityStats
        {
            get;
            set;
        } = new SecurityStatistics();

        /// <summary>
        /// Each specified sectors performance.
        /// </summary>
        public List<SectorStatistics> SectorStats
        {
            get;
            set;
        } = new List<SectorStatistics>();

        /// <summary>
        /// Value held in each bank account.
        /// </summary>
        public List<DayValue_Named> BankAccountStats
        {
            get;
            set;
        } = new List<DayValue_Named>();

        /// <summary>
        /// Statistics for each company holdin bank accounts.
        /// </summary>
        public List<DayValue_Named> BankAccountCompanyStats
        {
            get;
            set;
        } = new List<DayValue_Named>();

        public DayValue_Named BankAccountTotalStats
        {
            get;
            set;
        } = new DayValue_Named();

        /// <summary>
        /// Default empty constructor.
        /// </summary>
        public PortfolioStatistics()
        {
        }

        /// <summary>
        /// Constructor from a portfolio.
        /// </summary>
        public PortfolioStatistics(IPortfolio portfolio)
        {
            fDatabaseName = portfolio.DatabaseName;
            GenerateStatistics(portfolio);
        }

        private void GenerateStatistics(IPortfolio portfolio)
        {
            Totals.Add(new DayValue_Named("Securities", string.Empty, DateTime.Today, portfolio.TotalValue(Account.Security).Truncate()));
            Totals.Add(new DayValue_Named("Bank Accounts", string.Empty, DateTime.Today, portfolio.TotalValue(Account.BankAccount).Truncate()));
            Totals.Add(new DayValue_Named("Portfolio", string.Empty, DateTime.Today, portfolio.TotalValue(DateTime.Today).Truncate()));

            IndividualSecurityStats = portfolio.GenerateFundsStatistics();

            CompanyTotalsStats = portfolio.GenerateCompanyOverviewStatistics();
            PortfolioSecurityStats = portfolio.GeneratePortfolioStatistics();

            BankAccountStats = portfolio.GenerateBankAccountStatistics(false);

            BankAccountCompanyStats = portfolio.BankAccountCompanyStatistics();
            BankAccountTotalStats = portfolio.GenerateBankAccountTotalStatistics();

            List<string> sectorNames = portfolio.GetSecuritiesSectors();
            foreach (string sectorName in sectorNames)
            {
                SectorStats.Add(portfolio.GenerateSectorFundsStatistics(sectorName));
                SectorStats.Add(portfolio.GenerateBenchMarkStatistics(sectorName));
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
                    fileWriter.CreateHTMLHeader($"Statement for funds as of {DateTime.Today.ToShortDateString()}", options.Colours);
                    fileWriter.WriteLine($"<h1>{fDatabaseName} - Statement on {DateTime.Today.ToShortDateString()}</h1>");
                }

                fileWriter.WriteTable(exportType, options.BankAccDataToExport, Totals, false);

                if (options.ShowSecurites)
                {
                    fileWriter.WriteTitle(exportType, "Funds Data", HtmlTag.h2);
                    List<SecurityStatistics> securityDataToWrite = IndividualSecurityStats;

                    if (options.DisplayValueFunds)
                    {
                        _ = securityDataToWrite.RemoveAll(entry => entry.LatestVal.Equals(0));
                    }

                    foreach (SecurityStatistics companyStatistic in CompanyTotalsStats)
                    {
                        int number = securityDataToWrite.Count(datum => datum.Company.Equals(companyStatistic.Company));
                        if (number > 1)
                        {
                            securityDataToWrite.Add(companyStatistic);
                        }
                    }

                    securityDataToWrite.SortSecurityStatistics(options.SecuritySortingField, options.SecuritySortDirection);

                    securityDataToWrite.Add(PortfolioSecurityStats);
                    if (options.Spacing)
                    {
                        if (options.SecuritySortingField == "Names" || options.SecuritySortingField == "Name" || options.SecuritySortingField == "Company" || string.IsNullOrEmpty(options.SecuritySortingField))
                        {
                            int index = 0;
                            while (index < securityDataToWrite.Count - 1)
                            {
                                if (securityDataToWrite[index].Names.Company != securityDataToWrite[index + 1].Names.Company)
                                {

                                    securityDataToWrite.Insert(index + 1, null);
                                    index++;
                                }
                                index++;
                            }
                        }
                    }

                    fileWriter.WriteTable(exportType, options.SecurityDataToExport, securityDataToWrite, true);
                }

                if (options.ShowBankAccounts)
                {
                    fileWriter.WriteTitle(exportType, "Bank Accounts Data", HtmlTag.h2);
                    List<DayValue_Named> bankAccountDataToWrite = BankAccountStats;

                    if (options.DisplayValueFunds)
                    {
                        _ = bankAccountDataToWrite.RemoveAll(entry => entry.Value.Equals(0));
                    }

                    foreach (DayValue_Named companyStatistic in BankAccountCompanyStats)
                    {
                        int number = bankAccountDataToWrite.Count(datum => datum.Names.Company.Equals(companyStatistic.Names.Company));
                        if (number > 1)
                        {
                            bankAccountDataToWrite.Add(companyStatistic);
                        }
                    }

                    bankAccountDataToWrite.SortName(options.BankAccountSortingField, options.BankSortDirection);
                    bankAccountDataToWrite.Add(BankAccountTotalStats);

                    if (options.Spacing)
                    {
                        if (options.BankAccountSortingField == "Names" || string.IsNullOrEmpty(options.BankAccountSortingField))
                        {
                            int index = 0;
                            while (index < bankAccountDataToWrite.Count - 1)
                            {
                                if (bankAccountDataToWrite[index].Names.Company != bankAccountDataToWrite[index + 1].Names.Company)
                                {
                                    bankAccountDataToWrite.Insert(index + 1, null);
                                    index++;
                                }
                                index++;
                            }
                        }
                    }

                    fileWriter.WriteTable(exportType, options.BankAccDataToExport, bankAccountDataToWrite, true);
                }

                if (options.ShowSectors)
                {
                    fileWriter.WriteTitle(exportType, "Analysis By Sector", HtmlTag.h2);
                    List<SectorStatistics> sectorDataToWrite = SectorStats;

                    if (options.DisplayValueFunds)
                    {
                        _ = sectorDataToWrite.RemoveAll(entry => entry.LatestVal.Equals(0));
                    }

                    sectorDataToWrite.SortSectorStatistics(options.SectorSortingField, options.SectorSortDirection);
                    if (options.Spacing)
                    {
                        if (options.SectorSortingField == "Names" || options.SectorSortingField == "Name" || options.SectorSortingField == "Company" || string.IsNullOrEmpty(options.SectorSortingField))
                        {
                            sectorDataToWrite.Sort((a, b) => a.Name.CompareTo(b.Name));
                            int i = 0;
                            while (i < sectorDataToWrite.Count - 1)
                            {
                                if (sectorDataToWrite[i].Names.Name != sectorDataToWrite[i + 1].Names.Name)
                                {
                                    sectorDataToWrite.Insert(i + 1, null);
                                    i++;
                                }
                                i++;
                            }
                        }
                    }

                    fileWriter.WriteTable(exportType, options.SectorDataToExport, sectorDataToWrite, true);
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
