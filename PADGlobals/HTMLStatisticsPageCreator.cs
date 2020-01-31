﻿using FinancialStructures.DataStructures;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.Database;
using System;
using System.Collections.Generic;
using System.IO;
using FinancialStructures.FinanceStructures;

namespace PortfolioStatsCreatorHelper
{
    public enum ExportType
    {
        HTML,
        CSV
    }

    public static class PortfolioStatsCreators
    {

        private static void WriteSpacing(StreamWriter writer, bool spacing)
        {
            if (spacing)
            {
                writer.WriteLine("");
            }
        }

        private static void WriteSectorAnalysis(StreamWriter writer, Portfolio portfolio, List<Sector> sectors, UserOptions options, int maxNameLength, int maxCompanyLength, int maxNumLength)
        {
            writer.WriteLine("<h2>Analysis By Sector</h2>");

            writer.WriteLine("<table>");
            writer.WriteLine("<caption>Breakdown by sector of money in portfolio</caption>");
            writer.WriteLine("<thead><tr>");

            SecurityStatsHolder temp = new SecurityStatsHolder();
            writer.WriteLine(temp.HtmlTableHeader(options, options.SecurityDataToExport));
            writer.WriteLine("</tr></thead>");
            writer.WriteLine("<tbody>");

            List<string> sectorNames = portfolio.GetSecuritiesSectors();
            foreach (string sectorName in sectorNames)
            {
                List<SecurityStatsHolder> valuesToWrite = new List<SecurityStatsHolder>();
                valuesToWrite.Add(portfolio.GenerateSectorFundsStatistics(sectors, sectorName));
                valuesToWrite.Add(portfolio.GenerateBenchMarkStatistics(sectors, sectorName));
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

                if (linesWritten > 0)
                {
                    writer.WriteLine($"<tr><td><br/></td></tr>");
                }
            }

            writer.WriteLine("</tbody>");
            writer.WriteLine("</table>");
        }

        public static bool CreateHTMLPageCustom(Portfolio portfolio, List<Sector> sectors, string filepath, UserOptions options)
        {
            return CreateNewHTMLPage(portfolio, sectors, filepath, options);
        }
        private static bool CreateNewHTMLPage(Portfolio portfolio, List<Sector> sectors, string filepath, UserOptions options)
        {

            int maxNameLength = Math.Min(25, portfolio.LongestName() + 2);
            int maxCompanyLength = Math.Min(25, portfolio.LongestCompany() + 2);
            int maxNumLength = 10;
            int length = maxNameLength + maxCompanyLength + 8 * maxNumLength;
            StreamWriter htmlWriter = new StreamWriter(filepath);
            CreateHTMLHeader(htmlWriter, length);

            DailyValuation_Named securityTotals = new DailyValuation_Named(string.Empty, "Securities", DateTime.Today, portfolio.AllSecuritiesValue(DateTime.Today));
            DailyValuation_Named bankTotals = new DailyValuation_Named(string.Empty, "Totals", DateTime.Today, portfolio.AllBankAccountsValue(DateTime.Today));
            DailyValuation_Named portfolioTotals = new DailyValuation_Named(string.Empty, "Portfolio", DateTime.Today, portfolio.Value(DateTime.Today));
            List<string> headersList = new List<string>();
            headersList.AddRange(options.BankAccDataToExport);
            headersList.Remove("Name");
            htmlWriter.WriteLine("<table width=\"Auto\">");
            htmlWriter.WriteLine("<caption>Total held in portfolio.</caption>");
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

            htmlWriter.WriteLine("<h2>Funds Data</h2>");
            htmlWriter.WriteLine("<table>");
            htmlWriter.WriteLine("<caption>Breakdown of securities held by company</caption>");
            htmlWriter.WriteLine("<thead><tr>");
            var totals = portfolio.GeneratePortfolioStatistics();
            
            htmlWriter.WriteLine(totals.HtmlTableHeader(options, options.SecurityDataToExport));
            htmlWriter.WriteLine("</tr></thead>");
            htmlWriter.WriteLine("<tbody>");
            List<string> companies = portfolio.GetSecuritiesCompanyNames();
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

                if (linesWritten > 0)
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

            htmlWriter.WriteLine("<h2>Bank Accounts Data</h2>");

            htmlWriter.WriteLine("<table>");
            htmlWriter.WriteLine("<caption>Breakdown of bank accounts held by company</caption>");
            htmlWriter.WriteLine("<thead><tr>");
         
            htmlWriter.WriteLine(bankTotals.HTMLTableHeader(options, options.BankAccDataToExport));
            htmlWriter.WriteLine("</tr></thead>");
            htmlWriter.WriteLine("<tbody>");


            List<string> BankCompanies = portfolio.GetBankAccountCompanyNames();
            foreach (string compName in BankCompanies)
            {
                var bankAccounts = portfolio.GenerateBankAccountStatistics(compName);
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

                if (linesWritten > 0)
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
            WriteSpacing(htmlWriter, options.Spacing);

            WriteSpacing(htmlWriter, options.Spacing);

            WriteSectorAnalysis(htmlWriter, portfolio, sectors, options, maxNameLength, maxCompanyLength, maxNumLength);

            CreateHTMLFooter(htmlWriter, length);
            htmlWriter.Close();

            return true;
        }


        private static void CreateHTMLHeader(StreamWriter writer, int length)
        {
            writer.WriteLine("<!DOCTYPE html>");
            writer.WriteLine("<html>");
            writer.WriteLine("<head>");
            writer.WriteLine($"<title> Statement for funds as of {DateTime.Today.ToShortDateString()}</title>");
            writer.WriteLine("<style>");
            writer.WriteLine("html, h1, h2, h3, h4, h5, h6 {font-family: \"Arial\", cursive, sans-serif; }");
            writer.WriteLine("h1 { font-family: \"Arial\", cursive, sans-serif; font-size:7;margin-top: 1.5em; }");
            writer.WriteLine("h2 { font-family: \"Arial\", cursive, sans-serif; margin-top: 1.5em; }");
            writer.WriteLine("body{ font-family: \"Arial\", cursive, sans-serif; font-size:small }");
            writer.WriteLine("table { Width: 90%; border-collapse: collapse; margin-left: 5%; margin-right: 5%;}");
            writer.WriteLine("table, th, td { border: 1px solid black; }");
            writer.WriteLine("caption { margin-bottom: 1.2em; font-family: \"Arial\", cursive, sans-serif; font-size:medium; }");
            writer.WriteLine("tr:nth-child(even) {background-color: #f0f8ff;}");
            writer.WriteLine("th{ background-color: #ADD8E6; height: 50px; }");
            writer.WriteLine(" p { line-height:2.2em; margin-bottom: 2.2em;}");
            writer.WriteLine("</style> ");

            writer.WriteLine("</head>");
            writer.WriteLine("<body>");

            writer.WriteLine($"<h1>{GlobalHeldData.GlobalData.DatabaseName} - Portfolio Statement</h1>");

            writer.WriteLine($"<p>Produced by Finance Portfolio Database on the date {DateTime.Today.ToShortDateString()}.</p>");

        }

        private static void CreateHTMLFooter(StreamWriter writer, int length)
        {
            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
        }
    }
}
