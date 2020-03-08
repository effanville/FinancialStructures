using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using System;
using System.Collections.Generic;

namespace FinancialStructures.PortfolioAPI
{
    public static class PortfolioAddMethods
    {
        /// <summary>
        /// Adds data to the portfolio, unless data already exists.
        /// </summary>
        /// <param name="portfolio">The database to add to.</param>
        /// <param name="elementType">The type of data to add.</param>
        /// <param name="name">The name data to add.</param>
        /// <param name="reportLogger">Report callback action.</param>
        /// <returns>Success or failure of adding.</returns>
        public static bool TryAdd(this Portfolio portfolio, PortfolioElementType elementType, NameData name, Action<string, string, string> reportLogger)
        {
            List<string> sectorList = new List<string>();
            if (!string.IsNullOrEmpty(name.Sectors))
            {
                var sectorsSplit = name.Sectors.Split(',');

                sectorList.AddRange(sectorsSplit);
                for (int i = 0; i < sectorList.Count; i++)
                {
                    sectorList[i] = sectorList[i].Trim(' ');
                }
            }

            if (string.IsNullOrEmpty(name.Name) && string.IsNullOrEmpty(name.Company))
            {
                reportLogger("Error", "AddingData", $"Adding {elementType}: Company `{name.Company}' or name `{name.Name}' cannot both be empty.");
                return false;
            }

            switch (elementType)
            {
                case (PortfolioElementType.Security):
                    return portfolio.TryAddSecurity(name.Company, name.Name, name.Currency, name.Url, sectorList, reportLogger);
                case (PortfolioElementType.Currency):
                    return portfolio.TryAddCurrency(name.Name, name.Url, reportLogger);
                case (PortfolioElementType.BankAccount):
                    return portfolio.TryAddBankAccount(name.Company, name.Name, name.Currency, sectorList, reportLogger);
                case (PortfolioElementType.Sector):
                default:
                    reportLogger("Error", "EditingData", $"Editing an Unknown type.");
                    return false;
            }
        }

        private static bool TryAddSecurity(this Portfolio portfolio, string company, string name, string currency, string url, List<string> sectors, Action<string, string, string> reportLogger)
        {
            if (portfolio.DoesSecurityExist(company, name))
            {
                reportLogger("Error", "AddingData", $"Security `{company}'-`{name}' already exists.");
                return false;
            }

            Security newSecurity = new Security(name, company, currency, url, sectors);
            portfolio.Funds.Add(newSecurity);
            reportLogger("Report", "AddingData", $"Security `{company}'-`{name}' added to database.");
            return true;
        }

        private static bool TryAddBankAccount(this Portfolio portfolio, string company, string name, string currency, List<string> sectors, Action<string, string, string> reportLogger)
        {
            if (portfolio.DoesBankAccountExistFromName(name, company))
            {
                reportLogger("Error", "AddingData", $"BankAccount `{company}'-`{name}' already exists.");
                return false;
            }

            var NewAccount = new CashAccount(name, company, currency);
            foreach (var sector in sectors)
            {
                NewAccount.TryAddSector(sector);
            }

            portfolio.BankAccounts.Add(NewAccount);
            reportLogger("Report", "AddingData", $"BankAccount `{company}'-`{name}' added to database.");
            return true;
        }

        private static bool TryAddCurrency(this Portfolio portfolio, string name, string url, Action<string, string, string> reportLogger)
        {
            foreach (var currency in portfolio.Currencies)
            {
                if (name == currency.GetName())
                {
                    reportLogger("Error", "AddingData", $"Currency '{name}' already exists.");
                    return false;
                }
            }
            Currency newSector = new Currency(name, url);
            reportLogger("Report", "AddingData", $"Currency '{name}' added to database.");
            portfolio.Currencies.Add(newSector);
            return true;
        }
    }
}
