using System;
using System.Collections.Generic;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;
using StructureCommon.Reporting;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public bool TryAddOrEditDataToSecurity(TwoName names, DateTime oldDate, DateTime date, double shares, double unitPrice, double Investment, IReportLogger reportLogger = null)
        {
            for (int fundIndex = 0; fundIndex < NumberOf(Account.Security); fundIndex++)
            {
                if (names.IsEqualTo(Funds[fundIndex].Names))
                {
                    _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.AddingData, $"Security `{names.Company}'-`{names.Name}' has data on date .");
                    return Funds[fundIndex].TryAddOrEditData(oldDate, date, unitPrice, shares, Investment, reportLogger);
                }
            }
            _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.AddingData, $"Security `{names.Company}'-`{names.Name}' could not be found in the database.");
            return false;
        }

        /// <inheritdoc/>
        public bool TryAddOrEditData(Account elementType, TwoName name, DailyValuation oldData, DailyValuation data, IReportLogger reportLogger = null)
        {
            switch (elementType)
            {
                case (Account.Security):
                {
                    return SingleListAddOrEdit(Funds, name, oldData, data, reportLogger);
                }
                case (Account.Currency):
                {
                    return SingleListAddOrEdit(Currencies, name, oldData, data, reportLogger);
                }
                case (Account.BankAccount):
                {
                    return SingleListAddOrEdit(BankAccounts, name, oldData, data, reportLogger);
                }
                case (Account.Benchmark):
                {
                    return SingleListAddOrEdit(BenchMarks, name, oldData, data, reportLogger);
                }
                default:
                    _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.AddingData, $"Editing an Unknown type.");
                    return false;
            }
        }

        private bool SingleListAddOrEdit<T>(List<T> listToEdit, TwoName name, DailyValuation oldData, DailyValuation data, IReportLogger reportLogger = null) where T : IValueList
        {
            for (int accountIndex = 0; accountIndex < listToEdit.Count; accountIndex++)
            {
                if (listToEdit[accountIndex].Names.IsEqualTo(name))
                {
                    // now edit data
                    return listToEdit[accountIndex].TryAddOrEditData(oldData.Day, data.Day, data.Value, reportLogger);
                }
            }

            return false;
        }
    }
}