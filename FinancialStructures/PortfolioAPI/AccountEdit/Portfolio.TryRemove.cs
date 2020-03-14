using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using System;

namespace FinancialStructures.PortfolioAPI
{
    public static class PortfolioDeleteMethods
    {
        /// <summary>
        /// Removes the account from the database if it can.
        /// </summary>
        /// <param name="portfolio">The database to query.</param>
        /// <param name="elementType">The type of account to remove.</param>
        /// <param name="name">The name of the account to remove.</param>
        /// <param name="reportLogger">A report callback.</param>
        /// <returns>Success or failure.</returns>
        public static bool TryRemove(this Portfolio portfolio, PortfolioElementType elementType, NameData name, Action<string, string, string> reportLogger)
        {
            if (string.IsNullOrEmpty(name.Name) && string.IsNullOrEmpty(name.Company))
            {
                reportLogger("Error", "RemovingData", $"Adding {elementType}: Company `{name.Company}' or name `{name.Name}' cannot both be empty.");
                return false;
            }

            switch (elementType)
            {
                case (PortfolioElementType.Security):
                    return portfolio.TryRemoveSecurity(name.Company, name.Name, reportLogger);
                case (PortfolioElementType.Currency):
                    return portfolio.TryRemoveCurrency(name, reportLogger);
                case (PortfolioElementType.BankAccount):
                    return portfolio.TryRemoveBankAccount(name, reportLogger);
                case (PortfolioElementType.Sector):
                default:
                    reportLogger("Error", "EditingData", $"Editing an Unknown type.");
                    return false;
            }
        }

        private static bool TryRemoveSecurity(this Portfolio portfolio, string company, string name, Action<string, string, string> reportLogger)
        {
            foreach (Security sec in portfolio.Funds)
            {
                if (sec.GetCompany() == company && sec.GetName() == name)
                {
                    portfolio.Funds.Remove(sec);
                    reportLogger("Report", "AddingData", $"Security `{company}'-`{name}' removed from the database.");
                    return true;
                }
            }
            reportLogger("Error", "AddingData", $"Security `{company}'-`{name}' could not be found in the database.");
            return false;
        }

        private static bool TryRemoveCurrency(this Portfolio portfolio, NameData name, Action<string, string, string> reportLogger)
        {
            foreach (var sector in portfolio.Currencies)
            {
                if (name.Name == sector.GetName())
                {
                    reportLogger("Report", "DeletingData", $"Deleted sector {sector.GetName()}");
                    portfolio.Currencies.Remove(sector);
                    return true;
                }
            }

            return false;
        }

        private static bool TryRemoveBankAccount(this Portfolio portfolio, NameData name, Action<string, string, string> reportLogger)
        {
            foreach (CashAccount acc in portfolio.BankAccounts)
            {
                if (acc.GetCompany() == name.Company && acc.GetName() == name.Name)
                {
                    portfolio.BankAccounts.Remove(acc);
                    reportLogger("Warning", "DeletingData", $"Deleting Bank Account: Deleted `{name.Company}'-`{name.Name}'.");
                    return true;
                }
            }
            reportLogger("Error", "DeletingData", $"Deleting Bank Account: Could not find account `{name.Company}'-`{name.Name}'.");
            return false;
        }
    }
}
