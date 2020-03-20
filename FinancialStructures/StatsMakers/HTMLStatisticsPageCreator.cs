using FinancialStructures.Database;
using FinancialStructures.DataStructures;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.PortfolioAPI;
using FinancialStructures.PortfolioStatsCreatorHelper;
using System;
using System.Collections.Generic;
using System.IO;

namespace FinancialStructures.StatsMakers
{
    public enum ExportType
    {
        HTML,
        CSV
    }

    public static class PortfolioStatsCreators
    {
        private static void WriteSectorAnalysis(StreamWriter writer, Portfolio portfolio, UserOptions options)
        {
            writer.WriteLine("<h2>Analysis By Sector</h2>");

            writer.WriteLine("<table>");
            writer.WriteLine("<thead><tr>");

            SecurityStatsHolder temp = new SecurityStatsHolder();
            writer.WriteLine(temp.HtmlTableHeader(options, options.SecurityDataToExport));
            writer.WriteLine("</tr></thead>");
            writer.WriteLine("<tbody>");

            List<string> sectorNames = portfolio.GetSecuritiesSectors();
            foreach (string sectorName in sectorNames)
            {
                List<SecurityStatsHolder> valuesToWrite = new List<SecurityStatsHolder>();
                valuesToWrite.Add(portfolio.GenerateSectorFundsStatistics(portfolio.BenchMarks, sectorName));
                valuesToWrite.Add(portfolio.GenerateBenchMarkStatistics(portfolio.BenchMarks, sectorName));
                int linesWritten = 0;
                foreach (var value in valuesToWrite)
                {
                    if ((options.DisplayValueFunds && value.LatestVal > 0) || !options.DisplayValueFunds)
                    {
                        linesWritten++;
                        writer.WriteLine("<tr>");
                        writer.WriteLine(value.HtmlTableData(options, options.SecurityDataToExport));
                        writer.WriteLine("</tr>");
                    }
                }

                if (options.Spacing && linesWritten > 0)
                {
                    writer.WriteLine($"<tr><td><br/></td></tr>");
                }
            }

            writer.WriteLine("</tbody>");
            writer.WriteLine("</table>");
        }

        public static bool CreateHTMLPageCustom(Portfolio portfolio, string filepath, UserOptions options)
        {
            int maxNameLength = Math.Min(25, portfolio.LongestName() + 2);
            int maxCompanyLength = Math.Min(25, portfolio.LongestCompany() + 2);
            int maxNumLength = 10;
            int length = maxNameLength + maxCompanyLength + 8 * maxNumLength;
            StreamWriter htmlWriter = new StreamWriter(filepath);
            CreateHTMLHeader(htmlWriter, portfolio.DatabaseName, options);

            DayValue_Named securityTotals = new DayValue_Named(string.Empty, "Securities", DateTime.Today, portfolio.TotalValue(AccountType.Security));
            DayValue_Named bankTotals = new DayValue_Named(string.Empty, "Totals", DateTime.Today, portfolio.TotalValue(AccountType.BankAccount));
            DayValue_Named portfolioTotals = new DayValue_Named(string.Empty, "Portfolio", DateTime.Today, portfolio.Value(DateTime.Today));
            List<string> headersList = new List<string>();
            headersList.AddRange(options.BankAccDataToExport);
            headersList.Remove("Name");
            htmlWriter.WriteLine("<table width=\"Auto\">");
            htmlWriter.WriteLine("<thead><tr>");
            htmlWriter.WriteLine(portfolioTotals.HTMLTableHeader(options, headersList));
            htmlWriter.WriteLine("</tr></thead>");
            htmlWriter.WriteLine("<tbody>");
            htmlWriter.WriteLine("<tr>");
            htmlWriter.WriteLine(securityTotals.HTMLTableData(options, headersList));
            htmlWriter.WriteLine("</tr>");
            htmlWriter.WriteLine("<tr>");
            htmlWriter.WriteLine(bankTotals.HTMLTableData(options, headersList));
            htmlWriter.WriteLine("</tr>");
            htmlWriter.WriteLine("<tr>");
            htmlWriter.WriteLine(portfolioTotals.HTMLTableData(options, headersList));
            htmlWriter.WriteLine("</tr>");
            htmlWriter.WriteLine("</tbody>");
            htmlWriter.WriteLine("</table>");

            if (options.ShowSecurites)
            {

                htmlWriter.WriteLine("<h2>Funds Data</h2>");
                htmlWriter.WriteLine("<table>");
                htmlWriter.WriteLine("<thead><tr>");
                var totals = portfolio.GeneratePortfolioStatistics();

                htmlWriter.WriteLine(totals.HtmlTableHeader(options, options.SecurityDataToExport));
                htmlWriter.WriteLine("</tr></thead>");
                htmlWriter.WriteLine("<tbody>");
                List<string> companies = portfolio.Companies(AccountType.Security);
                companies.Sort();
                foreach (string compName in companies)
                {
                    var securities = portfolio.GenerateCompanyFundsStatistics(compName);
                    int linesWritten = 0;
                    foreach (var sec in securities)
                    {
                        if ((options.DisplayValueFunds && sec.LatestVal > 0) || !options.DisplayValueFunds)
                        {
                            htmlWriter.WriteLine("<tr>");
                            htmlWriter.WriteLine(sec.HtmlTableData(options, options.SecurityDataToExport));
                            htmlWriter.WriteLine("</tr>");
                            linesWritten += 1;
                        }
                    }

                    if (options.Spacing && linesWritten > 0)
                    {
                        htmlWriter.WriteLine($"<tr><td><br/></td></tr>");
                    }
                }

                if ((options.DisplayValueFunds && totals.LatestVal > 0) || !options.DisplayValueFunds)
                {
                    htmlWriter.WriteLine("");
                    htmlWriter.WriteLine("<tr>");
                    htmlWriter.WriteLine(totals.HtmlTableData(options, options.SecurityDataToExport));
                    htmlWriter.WriteLine("</tr>");
                }

                htmlWriter.WriteLine("</tbody>");
                htmlWriter.WriteLine("</table>");
            }

            if (options.ShowBankAccounts)
            {
                htmlWriter.WriteLine("<h2>Bank Accounts Data</h2>");

                htmlWriter.WriteLine("<table>");
                htmlWriter.WriteLine("<thead><tr>");

                htmlWriter.WriteLine(bankTotals.HTMLTableHeader(options, options.BankAccDataToExport));
                htmlWriter.WriteLine("</tr></thead>");
                htmlWriter.WriteLine("<tbody>");


                List<string> BankCompanies = portfolio.Companies(AccountType.BankAccount);
                BankCompanies.Sort();
                foreach (string compName in BankCompanies)
                {
                    var bankAccounts = portfolio.GenerateCompanyBankAccountStatistics(compName, options.DisplayValueFunds);
                    int linesWritten = 0;
                    foreach (var acc in bankAccounts)
                    {
                        if ((options.DisplayValueFunds && acc.Value > 0) || !options.DisplayValueFunds)
                        {
                            linesWritten++;
                            htmlWriter.WriteLine("<tr>");
                            htmlWriter.WriteLine(acc.HTMLTableData(options, options.BankAccDataToExport));
                            htmlWriter.WriteLine("</tr>");
                        }
                    }

                    if (options.Spacing && linesWritten > 0)
                    {
                        htmlWriter.WriteLine($"<tr><td><br/></td></tr>");
                    }
                }


                if ((options.DisplayValueFunds && bankTotals.Value > 0) || !options.DisplayValueFunds)
                {
                    htmlWriter.WriteLine("<tr>");
                    htmlWriter.WriteLine(bankTotals.HTMLTableData(options, options.BankAccDataToExport));
                    htmlWriter.WriteLine("</tr>");
                }

                htmlWriter.WriteLine("</tbody>");
                htmlWriter.WriteLine("</table>");

            }
            if (options.ShowSectors)
            {
                WriteSectorAnalysis(htmlWriter, portfolio, options);
            }
            CreateHTMLFooter(htmlWriter, length);
            htmlWriter.Close();

            return true;
        }


        private static void CreateHTMLHeader(StreamWriter writer, string databaseName, UserOptions options)
        {
            writer.WriteLine("<!DOCTYPE html>");
            writer.WriteLine("<html>");
            writer.WriteLine("<head>");
            writer.WriteLine($"<title> Statement for funds as of {DateTime.Today.ToShortDateString()}</title>");
            writer.WriteLine("<style>");
            writer.WriteLine("html, h1, h2, h3, h4, h5, h6 {font-family: \"Arial\", cursive, sans-serif; }");
            writer.WriteLine("h1 { font-family: \"Arial\", cursive, sans-serif; margin-top: 1.5em; }");
            writer.WriteLine("h2 { font-family: \"Arial\", cursive, sans-serif; margin-top: 1.5em; }");
            writer.WriteLine("body{ font-family: \"Arial\", cursive, sans-serif; font-size: 10px }");
            writer.WriteLine("table { border-collapse: collapse;}");
            writer.WriteLine("table, th, td { border: 1px solid black; }");
            writer.WriteLine("caption { margin-bottom: 1.2em; font-family: \"Arial\", cursive, sans-serif; font-size:medium; }");
            writer.WriteLine("tr {text-align: center;}");
            if (options.Colours)
            {
                writer.WriteLine("tr:nth-child(even) {background-color: #f0f8ff;}");
                writer.WriteLine("th{ background-color: #ADD8E6; height: 1.5em; }");
            }
            else
            {
                writer.WriteLine("th{ height: 1.5em; }");
            }
            writer.WriteLine(" p { line-height: 1.5em; margin-bottom: 1.5em;}");
            writer.WriteLine("</style> ");

            writer.WriteLine("</head>");
            writer.WriteLine("<body>");

            writer.WriteLine($"<h1> {databaseName} - Statement on {DateTime.Today.ToShortDateString()}</h1>");

            //writer.WriteLine($"<p>Produced by Finance Portfolio Database on the date {DateTime.Today.ToShortDateString()}.</p>");

        }

        private static void CreateHTMLFooter(StreamWriter writer, int length)
        {
            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
        }
    }
}
