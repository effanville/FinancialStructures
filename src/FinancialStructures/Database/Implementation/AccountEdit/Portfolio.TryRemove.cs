using Effanville.Common.Structure.Reporting;
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
                    return _funds.Remove(name, reportLogger);
                }
                case Account.Currency:
                {
                    return _currencies.Remove(name, reportLogger);
                }
                case Account.BankAccount:
                {
                    return _bankAccounts.Remove(name, reportLogger);
                }
                case Account.Benchmark:
                {
                    return _benchmarks.Remove(name, reportLogger);
                }
                case Account.Asset:
                {
                    return _assets.Remove(name, reportLogger);
                }
                case Account.Pension:
                {
                    return _pensions.Remove(name, reportLogger);
                }
                case Account.Unknown:
                case Account.All:
                default:
                    reportLogger?.Log(ReportType.Error, ReportLocation.DeletingData.ToString(), $"Editing an Unknown type.");
                    return false;
            }
        }
    }
}