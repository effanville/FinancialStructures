using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Implementation
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

            if (Exists(accountType, name.ToTwoName()))
            {
                reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.AddingData.ToString(), $"{accountType}-{name} already exists.");
                OnPortfolioChanged(null, new PortfolioEventArgs(accountType));
                return false;
            }

            switch (accountType)
            {
                case Account.Security:
                {
                    return _funds.TryAdd(accountType, name, reportLogger);
                }
                case Account.Currency:
                {
                    if (string.IsNullOrEmpty(name.Company))
                    {
                        name.Company = "GBP";
                    }

                    return _currencies.TryAdd(accountType, name, reportLogger);
                }
                case Account.BankAccount:
                {
                    return _bankAccounts.TryAdd(accountType, name, reportLogger);
                }
                case Account.Benchmark:
                {
                    return _benchmarks.TryAdd(accountType, name, reportLogger);
                }
                case Account.Asset:
                {
                    return _assets.TryAdd(accountType, name, reportLogger);
                }
                case Account.Pension:
                {
                    return _pensions.TryAdd(accountType, name, reportLogger);
                }
                case Account.Unknown:
                case Account.All:
                default:
                    reportLogger?.Log(ReportType.Error, ReportLocation.AddingData.ToString(), $"Adding an Unknown type to portfolio.");
                    return false;
            }
        }
    }
}
