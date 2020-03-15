using FinancialStructures.Database;
using FinancialStructures.GUIFinanceStructures;
using System;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioData
    {
        /// <summary>
        /// Edits the data of the security, if possible.
        /// </summary>
        public static bool TryEditSecurityData(this Portfolio portfolio, Action<string, string, string> reportLogger, string company, string name, DateTime oldDate, DateTime newDate, double shares, double unitPrice, double Investment = 0)
        {
            for (int fundIndex = 0; fundIndex < portfolio.NumberOf(AccountType.Security); fundIndex++)
            {
                if (portfolio.Funds[fundIndex].GetCompany() == company && portfolio.Funds[fundIndex].GetName() == name)
                {
                    // now edit data
                    return portfolio.Funds[fundIndex].TryEditData(reportLogger, oldDate, newDate, shares, unitPrice, Investment);
                }
            }

            return false;
        }

        /// <summary>
        /// Attempts to edit data in the account.
        /// </summary>
        /// <param name="portfolio">The database to edit.</param>
        /// <param name="elementType">The type of data to edit.</param>
        /// <param name="name">The name to edit.</param>
        /// <param name="oldData">The data to edit.</param>
        /// /// <param name="newData">The data to edit.</param>
        /// <param name="reportLogger">Report callback.</param>
        /// <returns>Success or failure.</returns>
        /// <remarks> This cannot currently be used to add to securities due to different type of data.</remarks>
        public static bool TryEditData(this Portfolio portfolio, AccountType elementType, NameData name, DayValue_ChangeLogged oldData, DayValue_ChangeLogged newData, Action<string, string, string> reportLogger)
        {
            switch (elementType)
            {
                case (AccountType.Security):
                    {
                        return false;
                    }
                case (AccountType.Currency):
                    {
                        foreach (var currency in portfolio.Currencies)
                        {
                            if (name.Name == currency.GetName())
                            {
                                return currency.TryEditData(oldData.Day, newData.Day, newData.Value, reportLogger);
                            }
                        }

                        return false;
                    }
                case (AccountType.BankAccount):
                    {
                        for (int AccountIndex = 0; AccountIndex < portfolio.NumberOf(AccountType.BankAccount); AccountIndex++)
                        {
                            if (portfolio.BankAccounts[AccountIndex].GetCompany() == name.Company && portfolio.BankAccounts[AccountIndex].GetName() == name.Name)
                            {
                                // now edit data
                                return portfolio.BankAccounts[AccountIndex].TryEditData(oldData.Day, newData.Day, newData.Value, reportLogger);
                            }
                        }

                        reportLogger("Error", "EditingData", $"Editing BankAccount Data: Could not find bank account `{name.Company}'-`{name.Name}'.");
                        return false;
                    }
                case (AccountType.Sector):
                    {
                        for (int AccountIndex = 0; AccountIndex < portfolio.NumberOf(AccountType.Sector); AccountIndex++)
                        {
                            if (portfolio.BenchMarks[AccountIndex].GetName() == name.Name)
                            {
                                // now edit data
                                return portfolio.BenchMarks[AccountIndex].TryEditData(oldData.Day, newData.Day, newData.Value, reportLogger);
                            }
                        }

                        reportLogger("Error", "EditingData", $"Editing BenchMark Data: Could not find benchMark `{name.Company}'-`{name.Name}'.");
                        return false;
                    }
                default:
                    reportLogger("Error", "EditingData", $"Editing an Unknown type.");
                    return false;
            }
        }
    }
}
