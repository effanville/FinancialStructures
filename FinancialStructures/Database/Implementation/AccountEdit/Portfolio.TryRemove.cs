using System.Collections.Generic;
using System.Threading;

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
                    return RemoveAccount(_fundsBackingList, elementType, name, _fundsLock, reportLogger);
                }
                case Account.Currency:
                {
                    return RemoveAccount(_currenciesBackingList, elementType, name, _currenciesLock, reportLogger);
                }
                case Account.BankAccount:
                {
                    return RemoveAccount(_bankAccountBackingList, elementType, name, _bankAccountsLock, reportLogger);
                }
                case Account.Benchmark:
                {
                    return RemoveAccount(_benchMarksBackingList, elementType, name, _benchmarksLock, reportLogger);
                }
                case Account.Asset:
                {
                    return RemoveAccount(_assetsBackingList, elementType, name, _assetsLock, reportLogger);
                }
                case Account.Pension:
                {
                    return RemoveAccount(_pensionsBackingList, elementType, name, _pensionsLock, reportLogger);
                }
                default:
                    reportLogger?.Log(ReportType.Error, ReportLocation.DeletingData.ToString(),
                        $"Editing an Unknown type.");
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