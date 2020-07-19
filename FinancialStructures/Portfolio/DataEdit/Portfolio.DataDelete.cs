using System;
using System.Collections.Generic;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using StructureCommon.Reporting;

namespace FinancialStructures.Database
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public bool TryDeleteData(AccountType elementType, TwoName name, DateTime date, IReportLogger reportLogger = null)
        {
            switch (elementType)
            {
                case (AccountType.Security):
                {
                    for (int fundIndex = 0; fundIndex < NumberOf(AccountType.Security); fundIndex++)
                    {
                        if (name.IsEqualTo(Funds[fundIndex].Names))
                        {
                            // now edit data
                            return Funds[fundIndex].TryDeleteData(date, reportLogger);
                        }
                    }

                    break;
                }
                case (AccountType.Currency):
                {
                    return TryDeleteSingleListData(Currencies, elementType, name, date, reportLogger);
                }
                case (AccountType.BankAccount):
                {
                    return TryDeleteSingleListData(BankAccounts, elementType, name, date, reportLogger);
                }
                case (AccountType.Sector):
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

        private bool TryDeleteSingleListData<T>(List<T> values, AccountType elementType, TwoName name, DateTime date, IReportLogger reportLogger = null) where T : ISingleValueDataList
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
