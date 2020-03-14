using FinancialStructures.Database;
using FinancialStructures.GUIFinanceStructures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioData
    {
        /// <summary>
        /// Returns a list of all companes of the desired type in the databse.
        /// </summary>
        /// <param name="portfolio">Database to query.</param>
        /// <param name="elementType">Type of object to search for.</param>
        /// <param name="reportLogger">Report callback. (not used)</param>
        /// <returns>List of names of the desired type.</returns>
        public static List<string> Companies(this Portfolio portfolio, PortfolioElementType elementType, Action<string, string, string> reportLogger)
        {
            return portfolio.NameData(elementType, reportLogger).Select(NameData => NameData.Company).Distinct().ToList();
        }

        /// <summary>
        /// Returns a list of all names of the desired type in the databse.
        /// </summary>
        /// <param name="portfolio">Database to query.</param>
        /// <param name="elementType">Type of object to search for.</param>
        /// <param name="reportLogger">Report callback. (not used)</param>
        /// <returns>List of names of the desired type.</returns>
        public static List<string> Names(this Portfolio portfolio, PortfolioElementType elementType, Action<string, string, string> reportLogger)
        {
            return portfolio.NameData(elementType, reportLogger).Select(NameData => NameData.Name).ToList();
        }

        /// <summary>
        /// Returns a list of all namedata in the databse.
        /// </summary>
        /// <param name="portfolio">Database to query.</param>
        /// <param name="elementType">Type of object to search for.</param>
        /// <param name="reportLogger">Report callback. (not used)</param>
        /// <returns>List of names of the desired type.</returns>
        public static List<NameCompDate> NameData(this Portfolio portfolio, PortfolioElementType elementType, Action<string, string, string> reportLogger)
        {
            var namesAndCompanies = new List<NameCompDate>();
            switch (elementType)
            {
                case (PortfolioElementType.Security):
                    {
                        foreach (var security in portfolio.Funds)
                        {
                            DateTime date = DateTime.MinValue;
                            if (security.Any())
                            {
                                date = security.LatestValue().Day;
                            }

                            namesAndCompanies.Add(new NameCompDate(security.GetCompany(), security.GetName(), security.GetCurrency(), security.GetUrl(), security.GetSectors(), date, false));
                        }
                        break;
                    }
                case (PortfolioElementType.Currency):
                    {
                        if (portfolio.Currencies != null)
                        {
                            foreach (var currency in portfolio.Currencies)
                            {
                                DateTime date = DateTime.MinValue;
                                if (currency.Any())
                                {
                                    date = currency.LatestValue().Day;
                                }

                                namesAndCompanies.Add(new NameCompDate(string.Empty, currency.GetName(), string.Empty, currency.GetUrl(), new List<string>(), date, false));
                            }
                        }
                        break;
                    }
                case (PortfolioElementType.BankAccount):
                    {
                        foreach (var bankAcc in portfolio.BankAccounts)
                        {
                            DateTime date = DateTime.MinValue;
                            if (bankAcc.Any())
                            {
                                date = bankAcc.LatestValue().Day;
                            }

                            namesAndCompanies.Add(new NameCompDate(bankAcc.GetCompany(), bankAcc.GetName(), bankAcc.GetCurrency(), string.Empty, bankAcc.GetSectors(), date, false));
                        }
                        break;
                    }
                case (PortfolioElementType.Sector):
                    {
                        break;
                    }
                default:
                    break;
            }

            return namesAndCompanies;
        }
    }
}
