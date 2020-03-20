using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;
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
        /// <returns>List of names of the desired type.</returns>
        public static List<string> Companies(this Portfolio portfolio, AccountType elementType)
        {
            return portfolio.NameData(elementType).Select(NameData => NameData.Company).Distinct().ToList();
        }

        /// <summary>
        /// Returns a list of all names of the desired type in the databse.
        /// </summary>
        /// <param name="portfolio">Database to query.</param>
        /// <param name="elementType">Type of object to search for.</param>
        /// <returns>List of names of the desired type.</returns>
        public static List<string> Names(this Portfolio portfolio, AccountType elementType)
        {
            return portfolio.NameData(elementType).Select(NameData => NameData.Name).ToList();
        }

        /// <summary>
        /// Returns a list of all namedata in the databse.
        /// </summary>
        /// <param name="portfolio">Database to query.</param>
        /// <param name="elementType">Type of object to search for.</param>
        /// <returns>List of names of the desired type.</returns>
        public static List<NameCompDate> NameData(this Portfolio portfolio, AccountType elementType)
        {
            var namesAndCompanies = new List<NameCompDate>();
            switch (elementType)
            {
                case (AccountType.Security):
                    {
                        foreach (var security in portfolio.Funds)
                        {
                            DateTime date = DateTime.MinValue;
                            if (security.Any())
                            {
                                date = security.LatestValue().Day;
                            }

                            namesAndCompanies.Add(new NameCompDate(security.GetCompany(), security.GetName(), security.GetCurrency(), security.GetUrl(), security.GetSectors(), date));
                        }
                        break;
                    }
                case (AccountType.Currency):
                    {
                        return SingleDataNameObtainer(portfolio.Currencies);
                    }
                case (AccountType.BankAccount):
                    {
                        return SingleDataNameObtainer(portfolio.BankAccounts);
                    }
                case (AccountType.Sector):
                    {
                        return SingleDataNameObtainer(portfolio.BenchMarks);
                    }
                default:
                    break;
            }

            return namesAndCompanies;
        }

        private static List<NameCompDate> SingleDataNameObtainer<T>(List<T> objects) where T : SingleValueDataList
        {
            var namesAndCompanies = new List<NameCompDate>();
            if (objects != null)
            {
                foreach (var dataList in objects)
                {
                    DateTime date = DateTime.MinValue;
                    if (dataList.Any())
                    {
                        date = dataList.LatestValue().Day;
                    }

                    namesAndCompanies.Add(new NameCompDate(dataList.GetCompany(), dataList.GetName(), dataList.GetCurrency(), dataList.GetUrl(), dataList.GetSectors(), date));
                }
            }
            return namesAndCompanies;
        }
    }
}
