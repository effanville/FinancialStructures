using FinancialStructures.Database;
using FinancialStructures.DatabaseInterfaces;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.NamingStructures;
using FinancialStructures.ReportLogging;
using System.Collections.Generic;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioData
    {
        /// <summary>
        /// Queries for data for the security of name and company. 
        /// </summary>
        public static List<DayDataView> SecurityData(this IPortfolio portfolio, string company, string name)
        {
            foreach (var security in portfolio.Funds)
            {
                if (security.GetName() == name && security.GetCompany() == company)
                {
                    return security.GetDataForDisplay();
                }
            }

            return new List<DayDataView>();
        }

        /// <summary>
        /// Returns the 
        /// </summary>
        /// <param name="portfolio"></param>
        /// <param name="elementType"></param>
        /// <param name="name"></param>
        /// <param name="reportLogger"></param>
        /// <returns></returns>
        public static List<DayValue_ChangeLogged> NumberData(this IPortfolio portfolio, AccountType elementType, NameData name, LogReporter reportLogger)
        {
            switch (elementType)
            {
                case (AccountType.Security):
                    {
                        return new List<DayValue_ChangeLogged>();
                    }
                case (AccountType.Currency):
                    {
                        return SingleDataListDataObtainer(portfolio.Currencies, elementType, name, reportLogger);
                    }
                case (AccountType.BankAccount):
                    {
                        return SingleDataListDataObtainer(portfolio.BankAccounts, elementType, name, reportLogger);
                    }
                case (AccountType.Sector):
                    {
                        return SingleDataListDataObtainer(portfolio.BenchMarks, elementType, name, reportLogger);
                    }
                default:
                    return new List<DayValue_ChangeLogged>();
            }
        }

        private static List<DayValue_ChangeLogged> SingleDataListDataObtainer<T>(List<T> objects, AccountType elementType, NameData name, LogReporter reportLogger) where T : SingleValueDataList
        {
            foreach (var account in objects)
            {
                if (account.GetName() == name.Name && account.GetCompany() == name.Company)
                {
                    return account.GetDataForDisplay();
                }
            }

            reportLogger.Log("Error", "DatabaseAccess", $"Could not find {elementType.ToString()} - {name.ToString()}");
            return new List<DayValue_ChangeLogged>();
        }
    }
}
