using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.Mathematics;
using FinancialStructures.PortfolioStatsCreatorHelper;
using StringFunctions;
using System;
using System.Collections.Generic;

namespace FinancialStructures.GUIFinanceStructures
{

    public static class SecurityStatsGenerator
    {
        public static void AddSecurityStats(this Portfolio portfolio, SecurityStatsHolder a, DateTime date)
        {

            if (a.Name == "Totals" && string.IsNullOrEmpty(a.Company))
            {
                a.LatestVal = MathSupport.Truncate(portfolio.AllSecuritiesValue(date));
                a.RecentChange = 0.0;
                a.FundsFraction = 1.0;
                a.Profit = MathSupport.Truncate(portfolio.TotalProfit());
                a.CAR3M = MathSupport.Truncate(100 * portfolio.IRRPortfolio(date.AddMonths(-3), date));
                a.CAR6M = MathSupport.Truncate(100 * portfolio.IRRPortfolio(date.AddMonths(-6), date));
                a.CAR1Y = MathSupport.Truncate(100 * portfolio.IRRPortfolio(date.AddMonths(-12), date));
                a.CAR5Y = MathSupport.Truncate(100 * portfolio.IRRPortfolio(date.AddMonths(-60), date));
                a.CARTotal = MathSupport.Truncate(100 * portfolio.IRRPortfolio(portfolio.FirstValueDate(), date));
            }
            else if (a.Name == "Totals" && !string.IsNullOrEmpty(a.Company))
            {
                var c = a.Company;
                a.LatestVal = MathSupport.Truncate(portfolio.SecurityCompanyValue(c, date));
                a.RecentChange = MathSupport.Truncate(portfolio.CompanyRecentChange(c));
                a.FundsFraction = MathSupport.Truncate(portfolio.CompanyFraction(c, date), 4);
                a.Profit = MathSupport.Truncate(portfolio.CompanyProfit(c));
                a.CAR3M = MathSupport.Truncate(100 * portfolio.IRRCompany(c, date.AddMonths(-3), date));
                a.CAR6M = MathSupport.Truncate(100 * portfolio.IRRCompany(c, date.AddMonths(-6), date));
                a.CAR1Y = MathSupport.Truncate(100 * portfolio.IRRCompany(c, date.AddMonths(-12), date));
                a.CAR5Y = MathSupport.Truncate(100 * portfolio.IRRCompany(c, date.AddMonths(-60), date));
                a.CARTotal = MathSupport.Truncate(100 * portfolio.IRRCompanyTotal(c));
            }
            else
            {
                var c = a.Company;
                var n = a.Name;
                portfolio.TryGetSecurity(c, n, out Security des);
                a.Number = des.NumberSectors();
                a.LatestVal = MathSupport.Truncate(portfolio.SecurityLatestValue(c, n));
                a.RecentChange = MathSupport.Truncate(portfolio.RecentChange(c, n));
                a.FundsFraction = MathSupport.Truncate(portfolio.SecurityFraction(c, n, date), 4);
                a.Profit = MathSupport.Truncate(portfolio.Profit(c, n));
                a.CAR3M = MathSupport.Truncate(100 * portfolio.IRR(c, n, date.AddMonths(-3), date));
                a.CAR6M = MathSupport.Truncate(100 * portfolio.IRR(c, n, date.AddMonths(-6), date));
                a.CAR1Y = MathSupport.Truncate(100 * portfolio.IRR(c, n, date.AddMonths(-12), date));
                a.CAR5Y = MathSupport.Truncate(100 * portfolio.IRR(c, n, date.AddMonths(-60), date));
                a.CARTotal = MathSupport.Truncate(100 * portfolio.IRR(c, n));
            }
        }

        public static void AddSectorStats(this Portfolio portfolio, SecurityStatsHolder a, DateTime date, List<Sector> sectors)
        {
            if (a.Company == "BenchMark")
            {
                Sector chosenSector = null;
                foreach (var sector in sectors)
                {
                    if (a.Name == sector.GetName())
                    {
                        chosenSector = sector.Copy();
                    }
                }
                if (chosenSector != null)
                {
                    a.LatestVal = MathSupport.Truncate(chosenSector.LatestValue().Value);
                    a.FundsFraction = 0.0;
                    a.Profit = 0.0;
                    a.Number = portfolio.NumberSecuritiesInSector(chosenSector.GetName());
                    a.CAR3M = MathSupport.Truncate(100 * chosenSector.CAR(date.AddMonths(-3), date));
                    a.CAR6M = MathSupport.Truncate(100 * chosenSector.CAR(date.AddMonths(-6), date));
                    a.CAR1Y = MathSupport.Truncate(100 * chosenSector.CAR(date.AddMonths(-12), date));
                    a.CAR5Y = MathSupport.Truncate(100 * chosenSector.CAR(date.AddMonths(-60), date));
                    a.CARTotal = MathSupport.Truncate(100 * chosenSector.CAR(portfolio.FirstValueDate(), date));
                }
            }
            else
            {
                a.LatestVal = MathSupport.Truncate(portfolio.SectorValue(a.Name, date));
                a.FundsFraction = MathSupport.Truncate(portfolio.SectorFraction(a.Name, date), 4);
                a.Profit = MathSupport.Truncate(portfolio.SectorProfit(a.Name));
                a.CAR3M = MathSupport.Truncate(100 * portfolio.IRRSector(a.Name, date.AddMonths(-3), date));
                a.CAR6M = MathSupport.Truncate(100 * portfolio.IRRSector(a.Name, date.AddMonths(-6), date));
                a.CAR1Y = MathSupport.Truncate(100 * portfolio.IRRSector(a.Name, date.AddMonths(-12), date));
                a.CAR5Y = MathSupport.Truncate(100 * portfolio.IRRSector(a.Name, date.AddMonths(-60), date));
                a.CARTotal = MathSupport.Truncate(100 * portfolio.IRRSector(a.Name, portfolio.FirstValueDate(), date));
            }
        }
    }

    public class SecurityStatsHolder : IComparable
    {
        /// <summary>
        /// Returns the property names with suitable html tags surrounding to place in a table header.
        /// </summary>
        public string HtmlData(UserOptions options, int maxNumLength, int maxNameLength, int maxCompanyLength)
        {
            var properties = this.GetType().GetProperties();
            string htmlData = string.Empty;

            foreach (var props in properties)
            {
                if (options.SecurityDataToExport.Contains(props.Name))
                {

                    if (Double.TryParse(props.GetValue(this).ToString(), out double result))
                    {
                        htmlData += result.ToString().PadLeft(maxNumLength);
                    }
                    else
                    {
                        if (props.Name == "Name")
                        {
                            htmlData += props.GetValue(this).ToString().WithMaxLength(maxNameLength - 2).PadRight(maxNameLength);
                        }
                        else
                        {
                            htmlData += props.GetValue(this).ToString().WithMaxLength(maxCompanyLength - 2).PadRight(maxCompanyLength);
                        }
                    }

                }
            }

            return htmlData;
        }

        public string HtmlHeader(UserOptions options, List<string> names, int maxNumLength, int maxNameLength, int maxCompanyLength)
        {
            var properties = this.GetType().GetProperties();
            string header = string.Empty;

            foreach (var props in properties)
            {
                if (names.Contains(props.Name))
                {
                    if (props.PropertyType == typeof(string))
                    {
                        if (props.Name == "Name")
                        {
                            header += "<b>" + props.Name.WithMaxLength(maxNameLength - 2).PadRight(maxNameLength) + "</b>";
                        }
                        else
                        {
                            header += "<b>" + props.Name.WithMaxLength(maxCompanyLength - 2).PadRight(maxCompanyLength) + "</b>";
                        }

                    }

                    if (props.PropertyType == typeof(double))
                    {
                        header += "<b>" + props.Name.WithMaxLength(maxNumLength - 2).PadLeft(maxNumLength) + "</b>";
                    }
                }
            }
            return header;
        }

        /// <summary>
        /// Returns the property names with suitable html tags surrounding to place in a table header.
        /// </summary>
        public string HtmlTableData(UserOptions options, List<string> names)
        {
            var properties = this.GetType().GetProperties();
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
            var properties = this.GetType().GetProperties();
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
            if (obj is SecurityStatsHolder value)
            {
                if (Company == value.Company)
                {
                    return Name.CompareTo(value.Name);
                }

                return Company.CompareTo(value.Company);
            }

            return 0;
        }

        public SecurityStatsHolder()
        { }
        public SecurityStatsHolder(string n, string c)
        {
            Name = n;
            Company = c;
        }

        public string Company { get; set; }
        public string Name { get; set; }
        public double LatestVal { get; set; }
        public double RecentChange { get; set; }
        public double FundsFraction { get; set; }
        public int Number { get; set; }
        public double Profit { get; set; }
        public double CAR3M { get; set; }
        public double CAR6M { get; set; }
        public double CAR1Y { get; set; }
        public double CAR5Y { get; set; }
        public double CARTotal { get; set; }
    }
}
