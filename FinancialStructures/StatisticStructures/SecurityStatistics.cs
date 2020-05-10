using FinancialStructures.Database;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.Mathematics;
using FinancialStructures.NamingStructures;
using FinancialStructures.PortfolioAPI;
using FinancialStructures.StatsMakers;
using System;
using System.Collections.Generic;

namespace FinancialStructures.StatisticStructures
{
    /// <summary>
    /// Generates the 
    /// </summary>
    public static class SecurityStatsGenerator
    {
        public static void AddSecurityStats(this IPortfolio portfolio, SecurityStatistics securityStats, DateTime date)
        {

            if (securityStats.StatsType == StatisticsType.PortfolioTotal)
            {
                securityStats.LatestVal = MathSupport.Truncate(portfolio.TotalValue(AccountType.Security, date));
                securityStats.RecentChange = MathSupport.Truncate(portfolio.RecentChange());
                securityStats.FundsFraction = 1.0;
                securityStats.Profit = MathSupport.Truncate(portfolio.TotalProfit());
                securityStats.CAR3M = MathSupport.Truncate(100 * portfolio.IRRPortfolio(date.AddMonths(-3), date));
                securityStats.CAR6M = MathSupport.Truncate(100 * portfolio.IRRPortfolio(date.AddMonths(-6), date));
                securityStats.CAR1Y = MathSupport.Truncate(100 * portfolio.IRRPortfolio(date.AddMonths(-12), date));
                securityStats.CAR5Y = MathSupport.Truncate(100 * portfolio.IRRPortfolio(date.AddMonths(-60), date));
                securityStats.CARTotal = MathSupport.Truncate(100 * portfolio.IRRPortfolio(portfolio.FirstValueDate(), date));
            }
            else if (securityStats.StatsType == StatisticsType.CompanyTotal)
            {
                var company = securityStats.Names.Company;
                securityStats.LatestVal = MathSupport.Truncate(portfolio.CompanyValue(AccountType.Security, company, date));
                securityStats.RecentChange = MathSupport.Truncate(portfolio.CompanyRecentChange(company));
                securityStats.FundsFraction = MathSupport.Truncate(portfolio.CompanyFraction(company, date), 4);
                securityStats.Profit = MathSupport.Truncate(portfolio.CompanyProfit(company));
                securityStats.CAR3M = MathSupport.Truncate(100 * portfolio.IRRCompany(company, date.AddMonths(-3), date));
                securityStats.CAR6M = MathSupport.Truncate(100 * portfolio.IRRCompany(company, date.AddMonths(-6), date));
                securityStats.CAR1Y = MathSupport.Truncate(100 * portfolio.IRRCompany(company, date.AddMonths(-12), date));
                securityStats.CAR5Y = MathSupport.Truncate(100 * portfolio.IRRCompany(company, date.AddMonths(-60), date));
                securityStats.CARTotal = MathSupport.Truncate(100 * portfolio.IRRCompanyTotal(company));
            }
            else
            {
                TwoName names = securityStats.Names;
                portfolio.TryGetSecurity(names, out ISecurity des);
                securityStats.Number = des.NumberSectors();
                securityStats.LatestVal = MathSupport.Truncate(portfolio.LatestValue(AccountType.Security, names));
                securityStats.SharePrice = MathSupport.Truncate(portfolio.SecurityPrices(names, date, SecurityDataStream.SharePrice), 4);
                securityStats.RecentChange = MathSupport.Truncate(portfolio.RecentChange(names));
                securityStats.FundsFraction = MathSupport.Truncate(portfolio.SecurityFraction(names, date), 4);
                securityStats.Profit = MathSupport.Truncate(portfolio.Profit(names));
                securityStats.CAR3M = MathSupport.Truncate(100 * portfolio.IRR(names, date.AddMonths(-3), date));
                securityStats.CAR6M = MathSupport.Truncate(100 * portfolio.IRR(names, date.AddMonths(-6), date));
                securityStats.CAR1Y = MathSupport.Truncate(100 * portfolio.IRR(names, date.AddMonths(-12), date));
                securityStats.CAR5Y = MathSupport.Truncate(100 * portfolio.IRR(names, date.AddMonths(-60), date));
                securityStats.CARTotal = MathSupport.Truncate(100 * portfolio.IRR(names));
                securityStats.Sectors = des.Names.SectorsFlat;
            }
        }

        public static void AddSectorStats(this IPortfolio portfolio, SecurityStatistics securityStats, DateTime date)
        {
            if (securityStats.StatsType == StatisticsType.BenchMarkTotal)
            {
                portfolio.TryGetAccount(AccountType.Sector, securityStats.Names, out var chosenSector);
                if (chosenSector != null)
                {
                    securityStats.LatestVal = MathSupport.Truncate(chosenSector.LatestValue().Value);
                    securityStats.FundsFraction = 0.0;
                    securityStats.Profit = 0.0;
                    securityStats.Number = portfolio.NumberSecuritiesInSector(chosenSector.Name);
                    securityStats.CAR3M = MathSupport.Truncate(100 * chosenSector.CAR(date.AddMonths(-3), date));
                    securityStats.CAR6M = MathSupport.Truncate(100 * chosenSector.CAR(date.AddMonths(-6), date));
                    securityStats.CAR1Y = MathSupport.Truncate(100 * chosenSector.CAR(date.AddMonths(-12), date));
                    securityStats.CAR5Y = MathSupport.Truncate(100 * chosenSector.CAR(date.AddMonths(-60), date));
                    securityStats.CARTotal = MathSupport.Truncate(100 * chosenSector.CAR(portfolio.FirstValueDate(), date));
                }
            }
            else
            {
                var name = securityStats.Names.Name;
                securityStats.LatestVal = MathSupport.Truncate(portfolio.SectorValue(name, date));
                securityStats.FundsFraction = MathSupport.Truncate(portfolio.SectorFraction(name, date), 4);
                securityStats.Profit = MathSupport.Truncate(portfolio.SectorProfit(name));
                securityStats.CAR3M = MathSupport.Truncate(100 * portfolio.IRRSector(name, date.AddMonths(-3), date));
                securityStats.CAR6M = MathSupport.Truncate(100 * portfolio.IRRSector(name, date.AddMonths(-6), date));
                securityStats.CAR1Y = MathSupport.Truncate(100 * portfolio.IRRSector(name, date.AddMonths(-12), date));
                securityStats.CAR5Y = MathSupport.Truncate(100 * portfolio.IRRSector(name, date.AddMonths(-60), date));
                securityStats.CARTotal = MathSupport.Truncate(100 * portfolio.IRRSector(name, portfolio.FirstValueDate(), date));
            }
        }
    }

    public class SecurityStatistics : IComparable
    {
        public StatisticsType StatsType;
        /// <summary>
        /// Returns the property names with suitable html tags surrounding to place in a table header.
        /// </summary>
        public string HtmlTableData(UserOptions options, List<string> names)
        {
            var properties = GetType().GetProperties();
            string htmlData = "<th scope=\"row\">";

            for (int i = 0; i < properties.Length; i++)
            {
                if (names.Contains(properties[i].Name))
                {
                    if (i != 0)
                    {
                        htmlData += "<td>";
                    }

                    htmlData += properties[i].GetValue(this).ToString();
                    htmlData += "</td>";
                }
            }

            return htmlData;
        }

        /// <summary>
        /// Returns the property names with suitable html tags surrounding to place in a table header.
        /// </summary>
        /// <returns></returns>
        public string HtmlTableHeader(UserOptions options, List<string> names)
        {
            var properties = GetType().GetProperties();
            string htmlHeader = string.Empty;
            foreach (var property in properties)
            {
                if (names.Contains(property.Name))
                {
                    htmlHeader += "<th scope=\"col\">";
                    htmlHeader += property.Name;
                    htmlHeader += "</th>";
                }
            }

            return htmlHeader;
        }

        public int CompareTo(object obj)
        {
            if (obj is SecurityStatistics value)
            {
                return Names.CompareTo(value.Names);
            }

            return 0;
        }

        public SecurityStatistics()
        { }

        public SecurityStatistics(StatisticsType statsType, TwoName names)
        {
            StatsType = statsType;
            Names = names;
        }

        public TwoName Names;
        public string Company { get { return Names.Company; } }
        public string Name { get { return Names.Name; } }
        public double LatestVal { get; set; }
        public double SharePrice { get; set; }
        public double RecentChange { get; set; }
        public double FundsFraction { get; set; }
        public int Number { get; set; }
        public double Profit { get; set; }
        public double CAR3M { get; set; }
        public double CAR6M { get; set; }
        public double CAR1Y { get; set; }
        public double CAR5Y { get; set; }
        public double CARTotal { get; set; }
        public string Sectors { get; set; } = string.Empty;
    }
}
