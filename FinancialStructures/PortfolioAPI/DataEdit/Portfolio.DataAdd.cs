using FinancialStructures.Database;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.ReportLogging;
using System;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioData
    {
        /// <summary>
        /// Adds the desired data to the security if it can.
        /// </summary>
        public static bool TryAddDataToSecurity(this Portfolio portfolio, LogReporter reportLogger, string company, string name, DateTime date, double shares, double unitPrice, double Investment = 0)
        {
            for (int fundIndex = 0; fundIndex < portfolio.NumberOf(AccountType.Security); fundIndex++)
            {
                if (portfolio.Funds[fundIndex].GetCompany() == company && portfolio.Funds[fundIndex].GetName() == name)
                {
                    return portfolio.Funds[fundIndex].TryAddData(reportLogger, date, unitPrice, shares, Investment);
                }
            }
            reportLogger.LogDetailed("Critical", "Error", "AddingData", $"Security `{company}'-`{name}' could not be found in the database.");
            return false;
        }

        /// <summary>
        /// Attempts to add data to the account.
        /// </summary>
        /// <param name="portfolio">The database to add to.</param>
        /// <param name="elementType">The type of data to add to.</param>
        /// <param name="name">The name to add to.</param>
        /// <param name="data">The data to add.</param>
        /// <param name="reportLogger">Report callback.</param>
        /// <returns>Success or failure.</returns>
        /// <remarks> This cannot currently be used to add to securities due to different type of data.</remarks>
        public static bool TryAddData(this Portfolio portfolio, AccountType elementType, NameData name, DayValue_ChangeLogged data, LogReporter reportLogger)
        {
            switch (elementType)
            {
                case (AccountType.Security):
                    {
                        return false;
                    }
                case (AccountType.Currency):
                    {
                        foreach (var sector in portfolio.GetCurrencies())
                        {
                            if (name.Name == sector.GetName())
                            {
                                return sector.TryAddData(data.Day, data.Value, reportLogger);
                            }
                        }

                        return false;
                    }
                case (AccountType.BankAccount):
                    {
                        for (int accountIndex = 0; accountIndex < portfolio.NumberOf(AccountType.BankAccount); accountIndex++)
                        {
                            if (portfolio.BankAccounts[accountIndex].GetCompany() == name.Company && portfolio.BankAccounts[accountIndex].GetName() == name.Name)
                            {
                                // now edit data
                                return portfolio.BankAccounts[accountIndex].TryAddData(data.Day, data.Value, reportLogger);
                            }
                        }

                        return false;
                    }
                case (AccountType.Sector):
                    {
                        for (int accountIndex = 0; accountIndex < portfolio.NumberOf(AccountType.Sector); accountIndex++)
                        {
                            if (portfolio.BenchMarks[accountIndex].GetName() == name.Name)
                            {
                                // now edit data
                                return portfolio.BenchMarks[accountIndex].TryAddData(data.Day, data.Value, reportLogger);
                            }
                        }

                        return false;
                    }
                default:
                    reportLogger.Log("Error", "EditingData", $"Editing an Unknown type.");
                    return false;
            }
        }
    }
}