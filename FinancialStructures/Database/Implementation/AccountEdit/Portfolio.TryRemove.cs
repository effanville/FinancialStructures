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
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.DeletingData, $"Adding {elementType}: Company `{name.Company}' or name `{name.Name}' cannot both be empty.");
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
                default:
                    _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.DeletingData, $"Editing an Unknown type.");
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
                            _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.DeletingData, $"{account}-{name} removed from the database.");
                            OnPortfolioChanged(currentItems, new PortfolioEventArgs(account));
                            return true;
                        }
                    }
                }

                _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.DeletingData, $"{account} - {name} could not be found in the database.");
                return false;
            }
        }
    }
}
