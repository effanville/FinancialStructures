using System;
using System.Collections.Generic;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;
using StructureCommon.Reporting;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public bool TryDeleteData(Account elementType, TwoName name, DateTime date, IReportLogger reportLogger = null)
        {
            switch (elementType)
            {
                case (Account.Security):
                {
                    return TryDeleteSingleListData(FundsThreadSafe, elementType, name, date, reportLogger);
                }
                case (Account.Currency):
                {
                    return TryDeleteSingleListData(CurrenciesThreadSafe, elementType, name, date, reportLogger);
                }
                case (Account.BankAccount):
                {
                    return TryDeleteSingleListData(BankAccountsThreadSafe, elementType, name, date, reportLogger);
                }
                case (Account.Benchmark):
                {
                    return TryDeleteSingleListData(BenchMarksThreadSafe, elementType, name, date, reportLogger);
                }
                default:
                    _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.DeletingData, $"Editing an Unknown type.");
                    return false;
            }
        }

        private bool TryDeleteSingleListData<T>(IReadOnlyList<T> values, Account elementType, TwoName name, DateTime date, IReportLogger reportLogger = null) where T : IValueList
        {
            foreach (T account in values)
            {
                if (name.IsEqualTo(account.Names))
                {
                    return account.TryDeleteData(date, reportLogger);
                }
            }

            _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.DeletingData, $"Could not find {elementType} - {name}.");
            return false;
        }
    }
}
