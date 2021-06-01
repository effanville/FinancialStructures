using FinancialStructures.FinanceStructures.Implementation;
using FinancialStructures.NamingStructures;
using StructureCommon.Reporting;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public bool TryAdd(Account elementType, NameData name, IReportLogger reportLogger = null)
        {
            if (string.IsNullOrWhiteSpace(name.Name) && string.IsNullOrWhiteSpace(name.Company))
            {
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.AddingData, $"Adding {elementType}: Company '{name.Company}' and name '{name.Name}' cannot both be empty.");
                return false;
            }

            if (Exists(elementType, name))
            {
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.AddingData, $"{elementType} `{name.Company}'-`{name.Name}' already exists.");
                OnPortfolioChanged(null, new PortfolioEventArgs(elementType));
                return false;
            }

            switch (elementType)
            {
                case (Account.Security):
                {
                    Security toAdd = new Security(name);
                    toAdd.DataEdit += OnPortfolioChanged;
                    lock (FundsLock)
                    {
                        Funds.Add(toAdd);
                    }

                    OnPortfolioChanged(toAdd, new PortfolioEventArgs(elementType));
                    break;
                }
                case (Account.Currency):
                {
                    if (string.IsNullOrEmpty(name.Company))
                    {
                        name.Company = "GBP";
                    }
                    Currency toAdd = new Currency(name);
                    toAdd.DataEdit += OnPortfolioChanged;
                    lock (CurrenciesLock)
                    {
                        Currencies.Add(toAdd);
                    }

                    OnPortfolioChanged(toAdd, new PortfolioEventArgs(elementType));
                    break;
                }
                case (Account.BankAccount):
                {
                    CashAccount toAdd = new CashAccount(name);
                    toAdd.DataEdit += OnPortfolioChanged;
                    lock (BankAccountsLock)
                    {
                        BankAccounts.Add(toAdd);
                    }

                    OnPortfolioChanged(toAdd, new PortfolioEventArgs(elementType));
                    break;
                }
                case (Account.Benchmark):
                {
                    Sector toAdd = new Sector(name);
                    toAdd.DataEdit += OnPortfolioChanged;
                    lock (BenchmarksLock)
                    {
                        BenchMarks.Add(toAdd);
                    }

                    OnPortfolioChanged(toAdd, new PortfolioEventArgs(elementType));
                    break;
                }
                default:
                    _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.EditingData, $"Editing an Unknown type.");
                    return false;
            }

            _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.AddingData, $"{elementType} `{name.Company}'-`{name.Name}' added to database.");
            return true;
        }
    }
}
