using System;
using System.Collections.Generic;
using FinancialStructures.Database;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FinancialStructures.PortfolioAPI;
using FinancialStructures.StatsMakers;
using StructureCommon.Extensions;

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
                securityStats.LatestVal = portfolio.TotalValue(AccountType.Security, date).Truncate();
                securityStats.RecentChange = portfolio.RecentChange().Truncate();
                securityStats.FundsFraction = 1.0;
                securityStats.FundCompanyFraction = 1.0;
                securityStats.Profit = portfolio.TotalProfit().Truncate();
                securityStats.CAR3M = (100 * portfolio.IRRPortfolio(date.AddMonths(-3), date)).Truncate();
                securityStats.CAR6M = (100 * portfolio.IRRPortfolio(date.AddMonths(-6), date)).Truncate();
                securityStats.CAR1Y = (100 * portfolio.IRRPortfolio(date.AddMonths(-12), date)).Truncate();
                securityStats.CAR5Y = (100 * portfolio.IRRPortfolio(date.AddMonths(-60), date)).Truncate();
                securityStats.CARTotal = (100 * portfolio.IRRPortfolio(portfolio.FirstValueDate(), date)).Truncate();
            }
            else if (securityStats.StatsType == StatisticsType.CompanyTotal)
            {
                string company = securityStats.Names.Company;
                securityStats.LatestVal = portfolio.CompanyValue(AccountType.Security, company, date).Truncate();
                securityStats.RecentChange = portfolio.CompanyRecentChange(company).Truncate();
                securityStats.FundsFraction = portfolio.CompanyFraction(company, date).Truncate(4);
                securityStats.FundCompanyFraction = 1.0;
                securityStats.Profit = portfolio.CompanyProfit(company).Truncate();
                securityStats.CAR3M = (100 * portfolio.IRRCompany(company, date.AddMonths(-3), date)).Truncate();
                securityStats.CAR6M = (100 * portfolio.IRRCompany(company, date.AddMonths(-6), date)).Truncate();
                securityStats.CAR1Y = (100 * portfolio.IRRCompany(company, date.AddMonths(-12), date)).Truncate();
                securityStats.CAR5Y = (100 * portfolio.IRRCompany(company, date.AddMonths(-60), date)).Truncate();
                securityStats.CARTotal = (100 * portfolio.IRRCompanyTotal(company)).Truncate();
            }
            else
            {
                TwoName names = securityStats.Names;
                _ = portfolio.TryGetSecurity(names, out ISecurity des);
                securityStats.Number = des.NumberSectors();
                securityStats.LatestVal = portfolio.LatestValue(AccountType.Security, names).Truncate();
                securityStats.SharePrice = portfolio.SecurityPrices(names, date, SecurityDataStream.SharePrice).Truncate(4);
                securityStats.RecentChange = portfolio.RecentChange(names).Truncate();
                securityStats.FundsFraction = portfolio.SecurityFraction(names, date).Truncate(4);
                securityStats.FundCompanyFraction = portfolio.SecurityCompanyFraction(names, date).Truncate(4);
                securityStats.Profit = portfolio.Profit(names).Truncate();
                securityStats.CAR3M = (100 * portfolio.IRR(names, date.AddMonths(-3), date)).Truncate();
                securityStats.CAR6M = (100 * portfolio.IRR(names, date.AddMonths(-6), date)).Truncate();
                securityStats.CAR1Y = (100 * portfolio.IRR(names, date.AddMonths(-12), date)).Truncate();
                securityStats.CAR5Y = (100 * portfolio.IRR(names, date.AddMonths(-60), date)).Truncate();
                securityStats.CARTotal = (100 * portfolio.IRR(names)).Truncate();
                securityStats.Sectors = des.Names.SectorsFlat;
            }
        }

        public static void AddSectorStats(this IPortfolio portfolio, SecurityStatistics securityStats, DateTime date)
        {
            if (securityStats.StatsType == StatisticsType.BenchMarkTotal)
            {
                _ = portfolio.TryGetAccount(AccountType.Sector, securityStats.Names, out ISingleValueDataList chosenSector);
                if (chosenSector != null)
                {
                    securityStats.LatestVal = chosenSector.LatestValue().Value.Truncate();
                    securityStats.FundsFraction = 0.0;
                    securityStats.FundCompanyFraction = 0.0;
                    securityStats.Profit = 0.0;
                    securityStats.Number = portfolio.NumberSecuritiesInSector(chosenSector.Name);
                    securityStats.CAR3M = (100 * chosenSector.CAR(date.AddMonths(-3), date)).Truncate();
                    securityStats.CAR6M = (100 * chosenSector.CAR(date.AddMonths(-6), date)).Truncate();
                    securityStats.CAR1Y = (100 * chosenSector.CAR(date.AddMonths(-12), date)).Truncate();
                    securityStats.CAR5Y = (100 * chosenSector.CAR(date.AddMonths(-60), date)).Truncate();
                    securityStats.CARTotal = (100 * chosenSector.CAR(portfolio.FirstValueDate(), date)).Truncate();
                }
            }
            else
            {
                string name = securityStats.Names.Name;
                securityStats.LatestVal = portfolio.SectorValue(name, date).Truncate();
                securityStats.FundsFraction = portfolio.SectorFraction(name, date).Truncate(4);
                securityStats.FundCompanyFraction = 0.0;
                securityStats.Profit = portfolio.SectorProfit(name).Truncate();
                securityStats.CAR3M = (100 * portfolio.IRRSector(name, date.AddMonths(-3), date)).Truncate();
                securityStats.CAR6M = (100 * portfolio.IRRSector(name, date.AddMonths(-6), date)).Truncate();
                securityStats.CAR1Y = (100 * portfolio.IRRSector(name, date.AddMonths(-12), date)).Truncate();
                securityStats.CAR5Y = (100 * portfolio.IRRSector(name, date.AddMonths(-60), date)).Truncate();
                securityStats.CARTotal = (100 * portfolio.IRRSector(name, portfolio.FirstValueDate(), date)).Truncate();
            }
        }
    }

    /// <summary>
    /// Contains all statistics pertaining to a security.
    /// </summary>
    public class SecurityStatistics : IComparable
    {
        public StatisticsType StatsType;
        /// <summary>
        /// Returns the property names with suitable html tags surrounding to place in a table header.
        /// </summary>
        public string HtmlTableData(UserOptions options, List<string> names)
        {
            System.Reflection.PropertyInfo[] properties = GetType().GetProperties();
            string htmlData = "<th scope=\"row\">";

            for (int i = 0; i < properties.Length; i++)
            {
                bool isDouble = double.TryParse(properties[i].GetValue(this).ToString(), out double value);

                if (names.Contains(properties[i].Name))
                {
                    if (i != 0)
                    {
                        if (value < 0)
                        {
                            htmlData += "<td data-negative>";
                        }
                        else
                        {
                            htmlData += "<td>";
                        }
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
            System.Reflection.PropertyInfo[] properties = GetType().GetProperties();
            string htmlHeader = string.Empty;
            foreach (System.Reflection.PropertyInfo property in properties)
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
        {
        }

        public SecurityStatistics(StatisticsType statsType, TwoName names)
        {
            StatsType = statsType;
            Names = names;
        }

        public TwoName Names;
        public string Company
        {
            get
            {
                return Names.Company;
            }
        }
        public string Name
        {
            get
            {
                return Names.Name;
            }
        }
        public double LatestVal
        {
            get; set;
        }
        public double SharePrice
        {
            get; set;
        }
        public double RecentChange
        {
            get; set;
        }
        public double FundsFraction
        {
            get; set;
        }
        public double FundCompanyFraction
        {
            get;
            set;
        }
        public int Number
        {
            get; set;
        }
        public double Profit
        {
            get; set;
        }
        public double CAR3M
        {
            get; set;
        }
        public double CAR6M
        {
            get; set;
        }
        public double CAR1Y
        {
            get; set;
        }
        public double CAR5Y
        {
            get; set;
        }
        public double CARTotal
        {
            get; set;
        }
        public string Sectors { get; set; } = string.Empty;
    }
}
