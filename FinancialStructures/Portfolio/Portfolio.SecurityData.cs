using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinancialStructures.Database
{
    public static partial class PortfolioSecurity
    {
        /// <summary>
        /// Returns the security names.
        /// </summary>
        public static IEnumerable<string> SecurityNames(this Portfolio portfolio)
        {
            return portfolio.Funds.Select(security => security.GetName());
        }

        /// <summary>
        /// Return alphabetically ordered list of all companies without repetition.
        /// </summary>
        public static List<string> GetSecuritiesCompanyNames(this Portfolio portfolio)
        {
            var companies = new List<string>();
            foreach (var security in portfolio.Funds)
            {
                if (companies.IndexOf(security.GetCompany()) == -1)
                {
                    companies.Add(security.GetCompany());
                }
            }
            companies.Sort();

            return companies;
        }

        /// <summary>
        /// Return alphabetically ordered list of all sectors linked to securities without repetition.
        /// </summary>
        public static List<string> GetSecuritiesSectors(this Portfolio portfolio)
        {
            var companies = new List<string>();
            foreach (var security in portfolio.Funds)
            {
                var sectors = security.GetSectors();
                foreach (var sector in sectors)
                {
                    if (companies.IndexOf(sector) == -1)
                    {
                        companies.Add(sector);
                    }
                }
            }
            companies.Sort();

            return companies;
        }

        /// <summary>
        /// Returns the names and companies with latest evaluation date of all securities.
        /// </summary>
        public static List<NameCompDate> SecurityNamesAndCompanies(this Portfolio portfolio)
        {
            var namesAndCompanies = new List<NameCompDate>();

            foreach (var security in portfolio.Funds)
            {
                DateTime date = DateTime.MinValue;
                if (security.Any())
                {
                    date = security.LatestValue().Day;
                }

                namesAndCompanies.Add(new NameCompDate(security.GetName(), security.GetCompany(), security.GetCurrency(), security.GetUrl(), security.GetSectors(), date, false));
            }

            return namesAndCompanies;
        }

        /// <summary>
        /// Returns a copy of all securities in the portfolio
        /// </summary>
        public static List<Security> GetSecurities(this Portfolio portfolio)
        {
            var listOfFunds = new List<Security>();
            foreach (Security sec in portfolio.Funds)
            {
                listOfFunds.Add(sec.Copy());
            }
            return listOfFunds;
        }

        /// <summary>
        /// Outputs a copy of the security if it exists.
        /// </summary>
        public static bool TryGetSecurity(this Portfolio portfolio, string company, string name, out Security desired)
        {
            foreach (Security sec in portfolio.Funds)
            {
                if (sec.GetName() == name && sec.GetCompany() == company)
                {
                    desired = sec.Copy();
                    return true;
                }
            }
            desired = null;
            return false;
        }

        /// <summary>
        /// Queries for data for the security of name and company. 
        /// </summary>
        public static bool TryGetSecurityData(this Portfolio portfolio, string company, string name, out List<DayDataView> data)
        {
            data = new List<DayDataView>();
            foreach (Security sec in portfolio.Funds)
            {
                if (sec.GetName() == name && sec.GetCompany() == company)
                {
                    data = sec.GetDataForDisplay();
                    return true;
                }
            }

            return false;
        }

        public static double SecurityShares(this Portfolio portfolio, string company, string name, DateTime date)
        {
            foreach (Security sec in portfolio.Funds)
            {
                if (sec.GetName() == name && sec.GetCompany() == company)
                {
                    return sec.Shares.NearestEarlierValue(date).Value;
                }
            }

            return 0.0;
        }
    }
}
