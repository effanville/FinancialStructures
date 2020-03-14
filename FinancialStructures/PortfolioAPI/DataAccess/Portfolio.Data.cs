using FinancialStructures.Database;
using FinancialStructures.GUIFinanceStructures;
using System;
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
            foreach (Security sec in portfolio.Funds)
            {
                if (sec.GetName() == name && sec.GetCompany() == company)
                {
                    return sec.GetDataForDisplay();
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
        public static List<DayValue_ChangeLogged> NumberData(this Portfolio portfolio, PortfolioElementType elementType, NameData name, Action<string, string, string> reportLogger)
        {
            switch (elementType)
            {
                case (PortfolioElementType.Security):
                    {
                        return new List<DayValue_ChangeLogged>();
                    }
                case (PortfolioElementType.Currency):
                    {
                        foreach (var currency in portfolio.Currencies)
                        {
                            if (currency.GetName() == name.Name)
                            {
                                return currency.GetDataForDisplay();
                            }
                        }

                        reportLogger("Error", "DatabaseAccess", $"Could not find currency {name.Name}");
                        return new List<DayValue_ChangeLogged>();
                    }
                case (PortfolioElementType.BankAccount):
                    {
                        foreach (var acc in portfolio.BankAccounts)
                        {
                            if (acc.GetName() == name.Name && acc.GetCompany() == name.Company)
                            {
                                return acc.GetDataForDisplay();
                            }
                        }
                        reportLogger("Report", "DatabaseAccess", $"Bank account {name.ToString()} does not exist.");
                        return new List<DayValue_ChangeLogged>();
                    }
                case (PortfolioElementType.Sector):
                    {
                        return new List<DayValue_ChangeLogged>();
                    }
                default:
                    return new List<DayValue_ChangeLogged>();
            }
        }
    }
}
