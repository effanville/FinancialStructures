using System;
using System.Collections.Generic;
using System.IO;
using FinancialStructures.Database;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.FinanceStructures;
using FinancialStructures.PortfolioAPI;
using FinancialStructures.StatisticStructures;
using StructureCommon.Extensions;
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
        /// Each specified sectors performance.
        /// </summary>
        public List<SecurityStatistics> SectorStats
        {
            get;
            set;
        } = new List<SecurityStatistics>();

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
            Totals.Add(new DayValue_Named("Securities", string.Empty, DateTime.Today, portfolio.TotalValue(AccountType.Security).Truncate()));
            Totals.Add(new DayValue_Named("Bank Accounts", string.Empty, DateTime.Today, portfolio.TotalValue(AccountType.BankAccount).Truncate()));
            Totals.Add(new DayValue_Named("Portfolio", string.Empty, DateTime.Today, portfolio.Value(DateTime.Today).Truncate()));

            IndividualSecurityStats = portfolio.GenerateFundsStatistics();

            CompanyTotalsStats = portfolio.GenerateCompanyOverviewStatistics();
            CompanyTotalsStats.Add(portfolio.GeneratePortfolioStatistics());

            BankAccountStats = portfolio.GenerateBankAccountStatistics(false);

            BankAccountCompanyStats = portfolio.BankAccountCompanyStatistics();

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
            // Still to do.
            // Put in a break when each company finishes, if options.Spacing==true
            // writer.WriteLine($"<tr><td><br/></td></tr>");
            // Dont put in totals line if only one thing for company written.
            try
            {
                StreamWriter fileWriter = new StreamWriter(filePath);
                if (exportType == ExportType.Html)
                {
                    fileWriter.CreateHTMLHeader($"Statement for funds as of {DateTime.Today.ToShortDateString()}", options);
                    fileWriter.WriteLine($"<h1>{fDatabaseName} - Statement on {DateTime.Today.ToShortDateString()}</h1>");
                }

                fileWriter.WriteTable(exportType, options.BankAccDataToExport, Totals);

                if (options.ShowSecurites)
                {
                    fileWriter.WriteTitle(exportType, "Funds Data", HtmlTag.h2);
                    var securityDataToWrite = IndividualSecurityStats;
                    securityDataToWrite.AddRange(CompanyTotalsStats);

                    if (options.DisplayValueFunds)
                    {
                        _ = securityDataToWrite.RemoveAll(entry => entry.LatestVal.Equals(0));
                    }

                    SecurityStatsGenerator.SortSecurityStatistics(securityDataToWrite, options.SecuritySortingField);
                    fileWriter.WriteTable(exportType, options.SecurityDataToExport, securityDataToWrite);
                }

                if (options.ShowBankAccounts)
                {
                    fileWriter.WriteTitle(exportType, "Bank Accounts Data", HtmlTag.h2);
                    var bankAccountDataToWrite = BankAccountStats;
                    bankAccountDataToWrite.AddRange(BankAccountCompanyStats);

                    if (options.DisplayValueFunds)
                    {
                        _ = bankAccountDataToWrite.RemoveAll(entry => entry.Value.Equals(0));
                    }

                    if (options.BankAccountSortingField == "Value")
                    {
                        bankAccountDataToWrite.Sort((a, b) => b.Value.CompareTo(a.Value));
                    }
                    else if (options.BankAccountSortingField == "Day")
                    {
                        bankAccountDataToWrite.Sort((a, b) => b.Day.CompareTo(a.Day));
                    }
                    else
                    {
                        bankAccountDataToWrite.Sort();
                    }

                    fileWriter.WriteTable(exportType, options.BankAccDataToExport, bankAccountDataToWrite);
                }

                if (options.ShowSectors)
                {
                    fileWriter.WriteTitle(exportType, "Analysis By Sector", HtmlTag.h2);
                    var sectorDataToWrite = SectorStats;

                    if (options.DisplayValueFunds)
                    {
                        _ = sectorDataToWrite.RemoveAll(entry => entry.LatestVal.Equals(0));
                    }

                    SecurityStatsGenerator.SortSecurityStatistics(sectorDataToWrite, options.SectorSortingField);
                    fileWriter.WriteTable(exportType, options.SectorDataToExport, sectorDataToWrite);
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
