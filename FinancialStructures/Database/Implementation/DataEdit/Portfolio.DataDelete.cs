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
                    for (int fundIndex = 0; fundIndex < NumberOf(Account.Security); fundIndex++)
                    {
                        if (name.IsEqualTo(Funds[fundIndex].Names))
                        {
                            // now edit data
                            return Funds[fundIndex].TryDeleteData(date, reportLogger);
                        }
                    }

                    break;
                }
                case (Account.Currency):
                {
                    return TryDeleteSingleListData(Currencies, elementType, name, date, reportLogger);
                }
                case (Account.BankAccount):
                {
                    return TryDeleteSingleListData(BankAccounts, elementType, name, date, reportLogger);
                }
                case (Account.Benchmark):
                {
                    return TryDeleteSingleListData(BenchMarks, elementType, name, date, reportLogger);
                }
                default:
                    _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.DeletingData, $"Editing an Unknown type.");
                    return false;
            }


            _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.DeletingData, $"Could not find {elementType.ToString()} - {name.ToString()}.");
            return false;
        }

        private bool TryDeleteSingleListData<T>(List<T> values, Account elementType, TwoName name, DateTime date, IReportLogger reportLogger = null) where T : IValueList
        {
            foreach (T account in values)
            {
                if (name.IsEqualTo(account.Names))
                {
                    return account.TryDeleteData(date, reportLogger);
                }
            }

            _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.DeletingData, $"Could not find {elementType.ToString()} - {name.ToString()}.");
            return false;
        }
    }
}
