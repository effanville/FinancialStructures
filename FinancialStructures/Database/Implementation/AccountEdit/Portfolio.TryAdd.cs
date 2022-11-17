using System.Collections.Generic;
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
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.AddingData, $"Adding {accountType}: Company '{name.Company}' and name '{name.Name}' cannot both be empty.");
                return false;
            }

            if (Exists(accountType, name))
            {
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.AddingData, $"{accountType}-{name} already exists.");
                OnPortfolioChanged(null, new PortfolioEventArgs(accountType));
                return false;
            }

            switch (accountType)
            {
                case Account.Security:
                {
                    AddAccount(accountType, new Security(name), Funds, FundsLock);
                    break;
                }
                case Account.Currency:
                {
                    if (string.IsNullOrEmpty(name.Company))
                    {
                        name.Company = "GBP";
                    }

                    AddAccount(accountType, new Currency(name), Currencies, CurrenciesLock);
                    break;
                }
                case Account.BankAccount:
                {
                    AddAccount(accountType, new CashAccount(name), BankAccounts, BankAccountsLock);
                    break;
                }
                case Account.Benchmark:
                {
                    AddAccount(accountType, new Sector(name), BenchMarks, BenchmarksLock);
                    break;
                }
                case Account.Asset:
                {
                    AddAccount(accountType, new AmortisableAsset(name), AssetsBackingList, AssetsLock);
                    break;
                }
                case Account.Pension:
                {
                    AddAccount(accountType, new Security(name), PensionsBackingList, PensionsLock);
                    break;
                }
                default:
                    _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.AddingData, $"Adding an Unknown type to portfolio.");
                    return false;
            }

            _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.AddingData, $"{accountType}-{name} added to database.");
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
