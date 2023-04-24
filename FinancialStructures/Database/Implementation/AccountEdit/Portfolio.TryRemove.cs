using System.Collections.Generic;

using Common.Structure.Reporting;

using FinancialStructures.FinanceStructures.Implementation;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public bool TryRemove(Account elementType, TwoName name, IReportLogger reportLogger = null)
        {
            if (string.IsNullOrEmpty(name.Name) && string.IsNullOrEmpty(name.Company))
            {
                reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.DeletingData.ToString(), $"Adding {elementType}: Company `{name.Company}' or name `{name.Name}' cannot both be empty.");
                return false;
            }

            switch (elementType)
            {
                case Account.Security:
                {
                    return RemoveAccount(Funds, elementType, name, FundsLock, reportLogger);
                }
                case Account.Currency:
                {
                    return RemoveAccount(Currencies, elementType, name, CurrenciesLock, reportLogger);
                }
                case Account.BankAccount:
                {
                    return RemoveAccount(BankAccounts, elementType, name, BankAccountsLock, reportLogger);
                }
                case Account.Benchmark:
                {
                    return RemoveAccount(BenchMarks, elementType, name, BenchmarksLock, reportLogger);
                }
                case Account.Asset:
                {
                    return RemoveAccount(AssetsBackingList, elementType, name, AssetsLock, reportLogger);
                }
                case Account.Pension:
                {
                    return RemoveAccount(PensionsBackingList, elementType, name, PensionsLock, reportLogger);
                }
                default:
                    reportLogger?.Log(ReportType.Error, ReportLocation.DeletingData.ToString(), $"Editing an Unknown type.");
                    return false;
            }

            bool RemoveAccount<T>(List<T> currentItems, Account account, TwoName name, object lockObject, IReportLogger reportLogger = null)
                where T : ValueList
            {
                lock (lockObject)
                {
                    foreach (T sec in currentItems)
                    {
                        if (name.IsEqualTo(sec.Names))
                        {
                            _ = currentItems.Remove(sec);
                            reportLogger?.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.DeletingData.ToString(), $"{account}-{name} removed from the database.");
                            OnPortfolioChanged(currentItems, new PortfolioEventArgs(account));
                            return true;
                        }
                    }
                }

                reportLogger?.Log(ReportType.Error, ReportLocation.DeletingData.ToString(), $"{account} - {name} could not be found in the database.");
                return false;
            }
        }
    }
}
