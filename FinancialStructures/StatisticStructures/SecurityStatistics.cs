using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Provides a comparer based on a provided field
        /// </summary>
        /// <param name="fieldToSortWith">The field one wishes to compare the statistics with.</param>
        /// <param name="direction">Whether to sort ascending or descending.</param>
        public static Comparison<SecurityStatistics> GetComparison(string fieldToSortWith, SortDirection direction)
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
                    if (direction == SortDirection.Descending)
                    {
                        return (a, b) => b.CompareTo(a);
                    }
                    else
                    {
                        return (a, b) => a.CompareTo(b);
                    }
                }
                case ("LatestVal"):
                {
                    if (direction == SortDirection.Descending)
                    {
                        return (a, b) => b.LatestVal.CompareTo(a.LatestVal);
                    }
                    else
                    {
                        return (a, b) => a.LatestVal.CompareTo(b.LatestVal);
                    }
                }
                case "SharePrice":
                {
                    if (direction == SortDirection.Descending)
                    {
                        return (a, b) => b.SharePrice.CompareTo(a.SharePrice);
                    }
                    else
                    {
                        return (a, b) => a.SharePrice.CompareTo(b.SharePrice);
                    }
                }
                case "RecentChange":
                {
                    if (direction == SortDirection.Descending)
                    {
                        return (a, b) => b.RecentChange.CompareTo(a.RecentChange);
                    }
                    else
                    {
                        return (a, b) => a.RecentChange.CompareTo(b.RecentChange);
                    }
                }
                case "Profit":
                {
                    if (direction == SortDirection.Descending)
                    {
                        return (a, b) => b.Profit.CompareTo(a.Profit);
                    }
                    else
                    {
                        return (a, b) => a.Profit.CompareTo(b.Profit);
                    }
                }
                case "CAR3M":
                {
                    if (direction == SortDirection.Descending)
                    {
                        return (a, b) => b.CAR3M.CompareTo(a.CAR3M);
                    }
                    else
                    {
                        return (a, b) => a.CAR3M.CompareTo(b.CAR3M);
                    }
                }
                case "CAR6M":
                {
                    if (direction == SortDirection.Descending)
                    {
                        return (a, b) => b.CAR6M.CompareTo(a.CAR6M);
                    }
                    else
                    {
                        return (a, b) => a.CAR6M.CompareTo(b.CAR6M);
                    }
                }
                case "CAR1Y":
                {
                    if (direction == SortDirection.Descending)
                    {
                        return (a, b) => b.CAR1Y.CompareTo(a.CAR1Y);
                    }
                    else
                    {
                        return (a, b) => a.CAR1Y.CompareTo(b.CAR1Y);
                    }
                }
                case "CAR5Y":
                {
                    if (direction == SortDirection.Descending)
                    {
                        return (a, b) => b.CAR5Y.CompareTo(a.CAR5Y);
                    }
                    else
                    {
                        return (a, b) => a.CAR5Y.CompareTo(b.CAR5Y);
                    }
                }
                case "CARTotal":
                {
                    if (direction == SortDirection.Descending)
                    {
                        return (a, b) => b.CARTotal.CompareTo(a.CARTotal);
                    }
                    else
                    {
                        return (a, b) => a.CARTotal.CompareTo(b.CARTotal);
                    }
                }
            }
        }

        /// <summary>
        /// Enacts a comparison of a list given a field to sort with.
        /// </summary>
        /// <param name="statsToSort"></param>
        /// <param name="fieldToSortUnder"></param>       
        /// <param name="direction">Whether to sort ascending or descending.</param>
        public static void SortSecurityStatistics(this List<SecurityStatistics> statsToSort, string fieldToSortUnder, SortDirection direction)
        {
            statsToSort.Sort(GetComparison(fieldToSortUnder, direction));
        }

        /// <summary>
        /// Adds statistics.
        /// </summary>
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
                securityStats.NumberShares = portfolio.SecurityPrices(names, date, SecurityDataStream.NumberOfShares).Truncate(4);
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

        /// <summary>
        /// Adds statistics for a sector.
        /// </summary>
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
                securityStats.Number = portfolio.NumberSecuritiesInSector(name);
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
        /// <summary>
        /// The type of statistics stored in this class.
        /// </summary>
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

        /// <summary>
        /// Constructor giving a type and a name.
        /// </summary>
        /// <param name="statsType"></param>
        /// <param name="names"></param>
        public SecurityStatistics(StatisticsType statsType, TwoName names)
        {
            StatsType = statsType;
            Names = names;
        }

        /// <summary>
        /// The names of this statistics.
        /// </summary>
        public TwoName Names;

        /// <summary>
        /// The company these statistics are associated to.
        /// </summary>
        public string Company
        {
            get
            {
                return Names.Company;
            }
        }

        /// <summary>
        /// The given name for these statistics.
        /// </summary>
        public string Name
        {
            get
            {
                return Names.Name;
            }
        }

        /// <summary>
        /// The latest value of the object.
        /// </summary>
        public double LatestVal
        {
            get;
            set;
        }

        /// <summary>
        /// The Share price of the object (if relevant).
        /// </summary>
        public double SharePrice
        {
            get;
            set;
        }

        public double NumberShares
        {
            get;
            set;
        }

        /// <summary>
        /// The change between the two most recent valuations.
        /// </summary>
        public double RecentChange
        {
            get;
            set;
        }

        /// <summary>
        /// The current fraction this object has out of all securities.
        /// </summary>
        public double FundsFraction
        {
            get;
            set;
        }

        /// <summary>
        /// The current fraction this object has out of all objects under company name.
        /// </summary>
        public double FundCompanyFraction
        {
            get;
            set;
        }

        /// <summary>
        /// Some miscellaneous field. Usually used for the number of sectors associated to this object.
        /// </summary>
        public int Number
        {
            get;
            set;
        }

        /// <summary>
        /// The profit gained from this object.
        /// </summary>
        public double Profit
        {
            get;
            set;
        }

        /// <summary>
        /// The IRR of this object over the past 3 months.
        /// </summary>
        public double CAR3M
        {
            get;
            set;
        }

        /// <summary>
        /// The IRR of this object over the past 6 months.
        /// </summary>
        public double CAR6M
        {
            get;
            set;
        }

        /// <summary>
        /// The IRR of this object over the past 1 year.
        /// </summary>
        public double CAR1Y
        {
            get;
            set;
        }

        /// <summary>
        /// The IRR of this object over the past 5 years.
        /// </summary>
        public double CAR5Y
        {
            get;
            set;
        }

        /// <summary>
        /// The IRR of this object over the entire history of holding it.
        /// </summary>
        public double CARTotal
        {
            get;
            set;
        }

        /// <summary>
        /// Any sectors this object is associated to.
        /// </summary>
        public string Sectors
        {
            get;
            set;
        } = string.Empty;
    }
}
