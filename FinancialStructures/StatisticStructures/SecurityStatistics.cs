using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FinancialStructures.Database;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FinancialStructures.PortfolioAPI;
using StructureCommon.Extensions;

namespace FinancialStructures.StatisticStructures
{
    /// <summary>
    /// Generates the 
    /// </summary>
    public static class SecurityStatsGenerator
    {
        public static Comparison<SecurityStatistics> GetComparison(string fieldToSortWith)
        {
            switch (fieldToSortWith)
            {
                case ("Company"):
                case ("Name"):
                case ("Names"):
                case "FundsFraction":
                case "FundCompanyFraction":
                case "Number":
                default:
                {
                    return (a, b) => a.CompareTo(b);
                }
                case ("LatestVal"):
                {
                    return (a, b) => b.LatestVal.CompareTo(a.LatestVal);
                }
                case "SharePrice":
                {
                    return (a, b) => b.SharePrice.CompareTo(a.SharePrice);
                }
                case "RecentChange":
                {
                    return (a, b) => b.RecentChange.CompareTo(a.RecentChange);
                }
                case "Profit":
                {
                    return (a, b) => b.Profit.CompareTo(a.Profit);
                }
                case "CAR3M":
                {
                    return (a, b) => b.CAR3M.CompareTo(a.CAR3M);
                }
                case "CAR6M":
                {
                    return (a, b) => b.CAR6M.CompareTo(a.CAR6M);
                }
                case "CAR1Y":
                {
                    return (a, b) => b.CAR1Y.CompareTo(a.CAR1Y);
                }
                case "CAR5Y":
                {
                    return (a, b) => b.CAR5Y.CompareTo(a.CAR5Y);
                }
                case "CARTotal":
                {
                    return (a, b) => b.CARTotal.CompareTo(a.CARTotal);
                }
            }
        }

        public static void SortSecurityStatistics(List<SecurityStatistics> statsToSort, string fieldToSortUnder)
        {
            statsToSort.Sort(GetComparison(fieldToSortUnder));
        }

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
        /// Default comparison of SecurityStatistics, comparing by name. 
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj is SecurityStatistics value)
            {
                if (StatsType == StatisticsType.PortfolioTotal)
                {
                    return 1;
                }
                if (value.StatsType == StatisticsType.PortfolioTotal)
                {
                    return -1;
                }

                if (StatsType == StatisticsType.CompanyTotal || value.StatsType == StatisticsType.CompanyTotal)
                {
                    if (Company.Equals(value.Company))
                    {
                        if (StatsType == StatisticsType.CompanyTotal)
                        {
                            return 1;
                        }
                        else
                        {
                            return -1;
                        }
                    }

                    return Company.CompareTo(value.Company);
                }

                return Names.CompareTo(value.Names);
            }

            return 0;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
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
