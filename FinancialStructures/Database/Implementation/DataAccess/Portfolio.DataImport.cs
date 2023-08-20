﻿using Common.Structure.Reporting;
using FinancialStructures.FinanceStructures;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public void ImportValuesFrom(IPortfolio other, IReportLogger reportLogger = null)
        {
            foreach (ISecurity security in Funds)
            {
                if (other.TryGetAccount(Account.Security, security.Names, out var otherSecurity)
                    && otherSecurity is ISecurity sec)
                {
                    foreach (var unitPrice in sec.UnitPrice.Values())
                    {
                        security.SetData(unitPrice.Day, unitPrice.Value, reportLogger);
                    }
                }
            }

            foreach (IExchangableValueList bankAccount in BankAccounts)
            {
                if (other.TryGetAccount(Account.BankAccount, bankAccount.Names, out var otherBankAccount)
                    && otherBankAccount is IExchangableValueList sec)
                {
                    foreach (var value in sec.Values.Values())
                    {
                        bankAccount.SetData(value.Day, value.Value, reportLogger);
                    }
                }
            }

            foreach (IValueList sector in BenchMarks)
            {
                if (other.TryGetAccount(Account.Benchmark, sector.Names, out var otherBankAccount)
                    && otherBankAccount is IValueList sec)
                {
                    foreach (var value in sec.Values.Values())
                    {
                        sector.SetData(value.Day, value.Value, reportLogger);
                    }
                }
            }

            foreach (ICurrency currency in Currencies)
            {
                if (other.TryGetAccount(Account.Currency, currency.Names, out var otherBankAccount)
                    && otherBankAccount is ICurrency sec)
                {
                    foreach (var value in sec.Values.Values())
                    {
                        currency.SetData(value.Day, value.Value, reportLogger);
                    }
                }
            }

            foreach (IAmortisableAsset asset in AssetsBackingList)
            {
                if (other.TryGetAccount(Account.Asset, asset.Names, out var otherBankAccount)
                    && otherBankAccount is IAmortisableAsset sec)
                {
                    foreach (var value in sec.Values.Values())
                    {
                        asset.SetData(value.Day, value.Value, reportLogger);
                    }
                }
            }

            foreach (ISecurity pension in PensionsBackingList)
            {
                if (other.TryGetAccount(Account.Pension, pension.Names, out var otherPension)
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