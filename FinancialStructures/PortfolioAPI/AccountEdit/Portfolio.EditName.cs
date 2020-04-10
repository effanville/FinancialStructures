using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FinancialStructures.Reporting;
using System.Collections.Generic;

namespace FinancialStructures.PortfolioAPI
{
    public static class PortfolioEditMethods
    {
        /// <summary>
        /// Edits the name of the data currently held.
        /// </summary>
        /// <param name="portfolio">The database to edit.</param>
        /// <param name="elementType">The type of data to edit.</param>
        /// <param name="oldName">The existing name of the data.</param>
        /// <param name="newName">The new name of the data.</param>
        /// <param name="reportLogger">Report callback.</param>
        /// <returns>Success or failure of editing.</returns>
        public static bool TryEditName(this IPortfolio portfolio, AccountType elementType, NameData oldName, NameData newName, IReportLogger reportLogger = null)
        {
            switch (elementType)
            {
                case (AccountType.Security):
                    {
                        for (int fundIndex = 0; fundIndex < portfolio.NumberOf(AccountType.Security); fundIndex++)
                        {
                            if (newName.IsEqualTo(portfolio.Funds[fundIndex].Names))
                            {
                                // now edit data
                                return portfolio.Funds[fundIndex].EditNameData(newName);
                            }
                        }
                        reportLogger?.LogUseful(ReportType.Error, ReportLocation.EditingData, $"Renaming {elementType.ToString()}: Could not find {elementType.ToString()} with name {oldName.ToString()}.");
                        return false;
                    }
                case (AccountType.Currency):
                    {
                        return TryEditNameSingleList(portfolio.Currencies, elementType, oldName, newName, reportLogger);
                    }
                case (AccountType.BankAccount):
                    {
                        return TryEditNameSingleList(portfolio.BankAccounts, elementType, oldName, newName, reportLogger);
                    }
                case (AccountType.Sector):
                    {
                        return TryEditNameSingleList(portfolio.BenchMarks, elementType, oldName, newName, reportLogger);
                    }
                default:
                    {
                        reportLogger.LogUseful(ReportType.Error, ReportLocation.EditingData, $"Editing an Unknown type.");
                        return false;
                    }
            }
        }

        private static bool TryEditNameSingleList<T>(List<T> values, AccountType elementType, NameData oldName, NameData newName, IReportLogger reportLogger = null) where T : ISingleValueDataList
        {
            for (int AccountIndex = 0; AccountIndex < values.Count; AccountIndex++)
            {
                if (values[AccountIndex].Names.IsEqualTo(oldName))
                {
                    // now edit data
                    return values[AccountIndex].EditNameData(newName);
                }
            }

            reportLogger?.LogUseful(ReportType.Error, ReportLocation.EditingData, $"Renaming {elementType.ToString()}: Could not find {elementType.ToString()} with name {oldName.ToString()}.");
            return false;
        }
    }
}
