using FinancialStructures.Database;
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
        public static List<DayDataView> SecurityData(this Portfolio portfolio, string company, string name)
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
        public static List<DayValue_ChangeLogged> NumberData(this Portfolio portfolio, AccountType elementType, NameData name, LogReporter reportLogger)
        {
            switch (elementType)
            {
                case (AccountType.Security):
                    {
                        return new List<DayValue_ChangeLogged>();
                    }
                case (AccountType.Currency):
                    {
                        foreach (var currency in portfolio.Currencies)
                        {
                            if (currency.GetName() == name.Name)
                            {
                                return currency.GetDataForDisplay();
                            }
                        }

                        break;
                    }
                case (AccountType.BankAccount):
                    {
                        foreach (var bankAccount in portfolio.BankAccounts)
                        {
                            if (bankAccount.GetName() == name.Name && bankAccount.GetCompany() == name.Company)
                            {
                                return bankAccount.GetDataForDisplay();
                            }
                        }

                        break;
                    }
                case (AccountType.Sector):
                    {
                        foreach (var sector in portfolio.GetBenchMarks())
                        {
                            if (sector.GetName() == name.Name)
                            {
                                return sector.GetDataForDisplay();
                            }
                        }

                        break;
                    }
                default:
                    return new List<DayValue_ChangeLogged>();
            }

            reportLogger.Log("Error", "DatabaseAccess", $"Could not find {elementType.ToString()} - {name.ToString()}");
            return new List<DayValue_ChangeLogged>();
        }
    }
}
