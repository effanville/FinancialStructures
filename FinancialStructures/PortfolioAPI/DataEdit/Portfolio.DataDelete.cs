using FinancialStructures.Database;
using FinancialStructures.GUIFinanceStructures;
using System;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioData
    {
        /// <summary>
        /// Attempts to remove data from the account.
        /// </summary>
        /// <param name="portfolio">The database to remove from.</param>
        /// <param name="elementType">The type of data to remove from.</param>
        /// <param name="name">The name to remove from.</param>
        /// <param name="data">The data to remove.</param>
        /// <param name="reportLogger">Report callback.</param>
        /// <returns>Success or failure.</returns>
        public static bool TryDeleteData(this Portfolio portfolio, AccountType elementType, NameData name, DayValue_ChangeLogged data, Action<string, string, string> reportLogger)
        {
            switch (elementType)
            {
                case (AccountType.Security):
                    {
                        for (int fundIndex = 0; fundIndex < portfolio.NumberOf(AccountType.Security); fundIndex++)
                        {
                            if (portfolio.Funds[fundIndex].GetCompany() == name.Company && portfolio.Funds[fundIndex].GetName() == name.Name)
                            {
                                // now edit data
                                return portfolio.Funds[fundIndex].TryDeleteData(reportLogger, data.Day);
                            }
                        }

                        reportLogger("Error", "DeletingData", $"Deleting Security Data: Could not find security `{name.Company}'-`{name.Name}'.");
                        return false;
                    }
                case (AccountType.Currency):
                    {
                        foreach (var currency in portfolio.Currencies)
                        {
                            if (name.Name == currency.GetName())
                            {
                                return currency.TryDeleteData(data.Day, reportLogger);
                            }
                        }

                        reportLogger("Error", "DeletingData", $"Deleting Currency Data: Could not find Currency  `{name.Company}'-`{name.Name}'.");
                        return false;
                    }
                case (AccountType.BankAccount):
                    {
                        for (int AccountIndex = 0; AccountIndex < portfolio.NumberOf(AccountType.BankAccount); AccountIndex++)
                        {
                            if (portfolio.BankAccounts[AccountIndex].GetCompany() == name.Company && portfolio.BankAccounts[AccountIndex].GetName() == name.Name)
                            {
                                // now edit data
                                return portfolio.BankAccounts[AccountIndex].TryDeleteData(data.Day, reportLogger);
                            }
                        }

                        reportLogger("Error", "DeletingData", $"Deleting Bank Account Data: Could not find bank account `{name.Company}'-`{name.Name}'.");
                        return false;
                    }
                case (AccountType.Sector):
                    {
                        foreach (var sector in portfolio.BenchMarks)
                        {
                            if (name.Name == sector.GetName())
                            {
                                return sector.TryDeleteData(data.Day, reportLogger);
                            }
                        }

                        reportLogger("Error", "DeletingData", $"Deleting Sector Data: Could not find Currency  `{name.Company}'-`{name.Name}'.");
                        return false;
                    }
                default:
                    reportLogger("Error", "EditingData", $"Editing an Unknown type.");
                    return false;
            }
        }
    }
}
