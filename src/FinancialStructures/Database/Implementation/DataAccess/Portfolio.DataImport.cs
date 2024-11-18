using Effanville.Common.Structure.DataStructures;
using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public void ImportValuesFrom(IPortfolio other, IReportLogger reportLogger = null)
        {
            foreach (ISecurity security in _funds.Values)
            {
                if (other.TryGetAccount(Account.Security, security.Names.ToTwoName(), out IValueList otherSecurity)
                    && otherSecurity is ISecurity sec)
                {
                    foreach (DailyValuation unitPrice in sec.UnitPrice.Values())
                    {
                        security.SetData(unitPrice.Day, unitPrice.Value, reportLogger);
                    }
                }
            }

            foreach (IExchangeableValueList bankAccount in _bankAccounts.Values)
            {
                if (other.TryGetAccount(Account.BankAccount, bankAccount.Names, out IValueList otherBankAccount)
                    && otherBankAccount is IExchangeableValueList sec)
                {
                    foreach (var value in sec.Values.Values())
                    {
                        bankAccount.SetData(value.Day, value.Value, reportLogger);
                    }
                }
            }

            foreach (IValueList sector in _benchmarks.Values)
            {
                if (other.TryGetAccount(Account.Benchmark, sector.Names, out IValueList otherBankAccount)
                    && otherBankAccount is IValueList sec)
                {
                    foreach (var value in sec.Values.Values())
                    {
                        sector.SetData(value.Day, value.Value, reportLogger);
                    }
                }
            }

            foreach (ICurrency currency in _currencies.Values)
            {
                if (other.TryGetAccount(Account.Currency, currency.Names, out IValueList otherBankAccount)
                    && otherBankAccount is ICurrency sec)
                {
                    foreach (var value in sec.Values.Values())
                    {
                        currency.SetData(value.Day, value.Value, reportLogger);
                    }
                }
            }

            foreach (IAmortisableAsset asset in _assets.Values)
            {
                if (other.TryGetAccount(Account.Asset, asset.Names, out IValueList otherBankAccount)
                    && otherBankAccount is IAmortisableAsset sec)
                {
                    foreach (var value in sec.Values.Values())
                    {
                        asset.SetData(value.Day, value.Value, reportLogger);
                    }
                }
            }

            foreach (ISecurity pension in _pensions.Values)
            {
                if (other.TryGetAccount(Account.Pension, pension.Names, out IValueList otherPension)
                    && otherPension is ISecurity sec)
                {
                    foreach (var unitPrice in sec.UnitPrice.Values())
                    {
                        pension.SetData(unitPrice.Day, unitPrice.Value, reportLogger);
                    }
                }
            }

            reportLogger?.Log(ReportSeverity.Critical, ReportType.Information, "DataImport", "Data imported from other database.");
        }
    }
}
