using FinancialStructures.Database;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.ReportLogging;
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
        public static bool TryEditName(this Portfolio portfolio, AccountType elementType, NameData oldName, NameData newName, LogReporter reportLogger)
        {
            List<string> sectorList = new List<string>();
            if (!string.IsNullOrEmpty(newName.Sectors))
            {
                var sectorsSplit = newName.Sectors.Split(',');
                sectorList.AddRange(sectorsSplit);
                for (int i = 0; i < sectorList.Count; i++)
                {
                    sectorList[i] = sectorList[i].Trim(' ');
                }
            }

            switch (elementType)
            {
                case (AccountType.Security):
                    {
                        for (int fundIndex = 0; fundIndex < portfolio.NumberOf(AccountType.Security); fundIndex++)
                        {
                            if (portfolio.Funds[fundIndex].GetCompany() == oldName.Company && portfolio.Funds[fundIndex].GetName() == oldName.Name)
                            {
                                // now edit data
                                return portfolio.Funds[fundIndex].EditNameData(newName.Company, newName.Name, newName.Currency, newName.Url, sectorList);
                            }
                        }
                        break;
                    }
                case (AccountType.Currency):
                    {
                        foreach (var currency in portfolio.Currencies)
                        {
                            if (currency.GetName() == oldName.Name)
                            {
                                currency.EditNameData(string.Empty, newName.Name, newName.Url);
                                return true;
                            }
                        }

                        break;
                    }
                case (AccountType.BankAccount):
                    {
                        for (int AccountIndex = 0; AccountIndex < portfolio.NumberOf(AccountType.BankAccount); AccountIndex++)
                        {
                            if (portfolio.BankAccounts[AccountIndex].GetCompany() == oldName.Company && portfolio.BankAccounts[AccountIndex].GetName() == oldName.Name)
                            {
                                // now edit data
                                return portfolio.BankAccounts[AccountIndex].EditNameData(newName.Company, newName.Name, string.Empty, newName.Currency, sectorList);
                            }
                        }

                        break;
                    }
                case (AccountType.Sector):
                    {
                        for (int AccountIndex = 0; AccountIndex < portfolio.NumberOf(AccountType.Sector); AccountIndex++)
                        {
                            if (portfolio.BankAccounts[AccountIndex].GetCompany() == oldName.Company && portfolio.BankAccounts[AccountIndex].GetName() == oldName.Name)
                            {
                                // now edit data
                                return portfolio.BenchMarks[AccountIndex].EditNameData(string.Empty, newName.Name, newName.Url);
                            }
                        }

                        break;
                    }
                default:
                    {
                        reportLogger.Log("Error", "EditingData", $"Editing an Unknown type.");
                        return false;
                    }
            }

            reportLogger.Log("Error", "EditingData", $"Renaming {elementType.ToString()}: Could not find {elementType.ToString()} with name {oldName.ToString()}.");
            return false;
        }
    }
}
