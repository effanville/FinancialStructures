using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.NamingStructures;
using FinancialStructures.ReportLogging;
using System;
using System.Collections.Generic;

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
                        return SingleListAdd(portfolio.Currencies, name, data, reportLogger);
                    }
                case (AccountType.BankAccount):
                    {
                        return SingleListAdd(portfolio.BankAccounts, name, data, reportLogger);
                    }
                case (AccountType.Sector):
                    {
                        return SingleListAdd(portfolio.BenchMarks, name, data, reportLogger);
                    }
                default:
                    reportLogger.Log("Error", "EditingData", $"Editing an Unknown type.");
                    return false;
            }
        }

        private static bool SingleListAdd<T>(List<T> listToEdit, NameData name, DayValue_ChangeLogged data, LogReporter reportLogger) where T : SingleValueDataList
        {
            for (int accountIndex = 0; accountIndex < listToEdit.Count; accountIndex++)
            {
                if (listToEdit[accountIndex].GetNameData().IsEqualTo(name))
                {
                    // now edit data
                    return listToEdit[accountIndex].TryAddData(data.Day, data.Value, reportLogger);
                }
            }

            return false;
        }
    }
}