using System.Collections.Generic;
using System.Threading;

using Common.Structure.Reporting;
using FinancialStructures.FinanceStructures.Implementation;
using FinancialStructures.FinanceStructures.Implementation.Asset;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public bool TryAdd(Account accountType, NameData name, IReportLogger reportLogger = null)
        {
            if (string.IsNullOrWhiteSpace(name.Name) && string.IsNullOrWhiteSpace(name.Company))
            {
                reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.AddingData.ToString(), $"Adding {accountType}: Company '{name.Company}' and name '{name.Name}' cannot both be empty.");
                return false;
            }

            if (Exists(accountType, name))
            {
                reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.AddingData.ToString(), $"{accountType}-{name} already exists.");
                OnPortfolioChanged(null, new PortfolioEventArgs(accountType));
                return false;
            }

            switch (accountType)
            {
                case Account.Security:
                {
                    AddAccount(accountType, new Security(name), _fundsBackingList, _fundsLock);
                    break;
                }
                case Account.Currency:
                {
                    if (string.IsNullOrEmpty(name.Company))
                    {
                        name.Company = "GBP";
                    }

                    AddAccount(accountType, new Currency(name), _currenciesBackingList, _currenciesLock);
                    break;
                }
                case Account.BankAccount:
                {
                    AddAccount(accountType, new CashAccount(name), _bankAccountBackingList, _bankAccountsLock);
                    break;
                }
                case Account.Benchmark:
                {
                    AddAccount(accountType, new Sector(name), _benchMarksBackingList, _benchmarksLock);
                    break;
                }
                case Account.Asset:
                {
                    AddAccount(accountType, new AmortisableAsset(name), _assetsBackingList, _assetsLock);
                    break;
                }
                case Account.Pension:
                {
                    AddAccount(accountType, new Security(name), _pensionsBackingList, _pensionsLock);
                    break;
                }
                default:
                    reportLogger?.Log(ReportType.Error, ReportLocation.AddingData.ToString(), $"Adding an Unknown type to portfolio.");
                    return false;
            }

            reportLogger?.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.AddingData.ToString(), $"{accountType}-{name} added to database.");
            return true;

            void AddAccount<T>(Account account, T newObject, List<T> currentItems, object lockObject)
                where T : ValueList
            {
                newObject.DataEdit += OnPortfolioChanged;
                lock (lockObject)
                {
                    currentItems.Add(newObject);
                }

                OnPortfolioChanged(newObject, new PortfolioEventArgs(account));
            }
        }
    }
}
