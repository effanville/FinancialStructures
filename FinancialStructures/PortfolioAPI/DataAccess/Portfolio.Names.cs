using FinancialStructures.FinanceInterfaces;
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
        public static List<string> Companies(this IPortfolio portfolio, AccountType elementType)
        {
            return portfolio.NameData(elementType).Select(NameData => NameData.Company).Distinct().ToList();
        }

        /// <summary>
        /// Returns a list of all names of the desired type in the databse.
        /// </summary>
        /// <param name="portfolio">Database to query.</param>
        /// <param name="elementType">Type of object to search for.</param>
        /// <returns>List of names of the desired type.</returns>
        public static List<string> Names(this IPortfolio portfolio, AccountType elementType)
        {
            return portfolio.NameData(elementType).Select(NameData => NameData.Name).ToList();
        }

        /// <summary>
        /// Returns a list of all namedata in the databse.
        /// </summary>
        /// <param name="portfolio">Database to query.</param>
        /// <param name="elementType">Type of object to search for.</param>
        /// <returns>List of names of the desired type.</returns>
        public static List<NameCompDate> NameData(this IPortfolio portfolio, AccountType elementType)
        {
            var namesAndCompanies = new List<NameCompDate>();
            switch (elementType)
            {
                case (AccountType.Security):
                {
                    foreach (ISecurity security in portfolio.Funds)
                    {
                        DateTime date = DateTime.MinValue;
                        if (security.Any())
                        {
                            date = security.LatestValue().Day;
                        }

                        namesAndCompanies.Add(new NameCompDate(security.Company, security.Name, security.Currency, security.Url, security.Sectors, date));
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

        private static List<NameCompDate> SingleDataNameObtainer<T>(List<T> objects) where T : ISingleValueDataList
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

                    namesAndCompanies.Add(new NameCompDate(dataList.Company, dataList.Name, dataList.Currency, dataList.Url, dataList.Names.Sectors, date));
                }
            }
            return namesAndCompanies;
        }
    }
}
