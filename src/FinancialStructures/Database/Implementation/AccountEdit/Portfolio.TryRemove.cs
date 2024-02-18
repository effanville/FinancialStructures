using System.Collections.Generic;
using System.Threading;

using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.FinanceStructures.Implementation;
using Effanville.FinancialStructures.FinanceStructures.Implementation.Asset;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Implementation
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
                    return RemoveAccount<Security>(_fundsDictionary, elementType, name, _fundsLock, reportLogger);
                }
                case Account.Currency:
                {
                    return RemoveAccount<Currency>(_currenciesDictionary, elementType, name, _currenciesLock, reportLogger);
                }
                case Account.BankAccount:
                {
                    return RemoveAccount<CashAccount>(_bankAccountsDictionary, elementType, name, _bankAccountsLock, reportLogger);
                }
                case Account.Benchmark:
                {
                    return RemoveAccount<Sector>(_benchMarksDictionary, elementType, name, _benchmarksLock, reportLogger);
                }
                case Account.Asset:
                {
                    return RemoveAccount<AmortisableAsset>(_assetsDictionary, elementType, name, _assetsLock, reportLogger);
                }
                case Account.Pension:
                {
                    return RemoveAccount<Security>(_pensionsDictionary, elementType, name, _pensionsLock, reportLogger);
                }
                case Account.Unknown:
                case Account.All:
                default:
                    reportLogger?.Log(ReportType.Error, ReportLocation.DeletingData.ToString(), $"Editing an Unknown type.");
                    return false;
            }
            bool RemoveAccount<T>(Dictionary<TwoName, T> currentItems, Account account, TwoName name, ReaderWriterLockSlim lockObject, IReportLogger reportLogger = null)
                where T : ValueList
            {
                lockObject.EnterWriteLock();
                try
                {
                    var nameToRemove = new TwoName(name.Company, name.Name);
                    if (currentItems.TryGetValue(nameToRemove, out var list))
                    {
                        list.DataEdit -= OnPortfolioChanged;
                    }

                    bool removed = currentItems.Remove(nameToRemove);
                    if (removed)
                    {
                        reportLogger?.Log(ReportSeverity.Detailed, ReportType.Information,
                            ReportLocation.DeletingData.ToString(), $"{account}-{name} removed from the database.");
                        OnPortfolioChanged(currentItems, new PortfolioEventArgs(account));
                        return true;
                    }
                }
                finally
                {
                    lockObject.ExitWriteLock();
                }

                reportLogger?.Log(ReportType.Error, ReportLocation.DeletingData.ToString(), $"{account} - {name} could not be found in the database.");
                return false;
            }
            
        }
    }
}