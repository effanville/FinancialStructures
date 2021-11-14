using System;
using System.Collections.Generic;

using Common.Structure.DataStructures;
using Common.Structure.Reporting;

using FinancialStructures.DataStructures;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public bool TryAddOrEditTradeData(Account elementType, TwoName names, SecurityTrade oldTrade, SecurityTrade newTrade, IReportLogger reportLogger = null)
        {
            IReadOnlyList<ISecurity> funds = FundsThreadSafe;
            for (int fundIndex = 0; fundIndex < NumberOf(Account.Security); fundIndex++)
            {
                if (names.IsEqualTo(funds[fundIndex].Names))
                {
                    return funds[fundIndex].TryAddOrEditTradeData(oldTrade, newTrade, reportLogger);
                }
            }
            _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.AddingData, $"Security `{names.Company}'-`{names.Name}' could not be found in the database.");
            return false;
        }

        /// <inheritdoc/>
        public bool TryAddOrEditDataToSecurity(TwoName names, DateTime oldDate, DateTime date, decimal shares, decimal unitPrice, decimal investment, SecurityTrade trade, IReportLogger reportLogger = null)
        {
            IReadOnlyList<ISecurity> funds = FundsThreadSafe;
            for (int fundIndex = 0; fundIndex < NumberOf(Account.Security); fundIndex++)
            {
                if (names.IsEqualTo(funds[fundIndex].Names))
                {
                    return funds[fundIndex].AddOrEditData(oldDate, date, unitPrice, shares, investment, trade, reportLogger);
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
                    return SingleListAddOrEdit(FundsThreadSafe, name, oldData, data, reportLogger);
                }
                case (Account.Currency):
                {
                    return SingleListAddOrEdit(CurrenciesThreadSafe, name, oldData, data, reportLogger);
                }
                case (Account.BankAccount):
                {
                    return SingleListAddOrEdit(BankAccountsThreadSafe, name, oldData, data, reportLogger);
                }
                case (Account.Benchmark):
                {
                    return SingleListAddOrEdit(BenchMarksThreadSafe, name, oldData, data, reportLogger);
                }
                default:
                    _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.AddingData, $"Editing an Unknown type.");
                    return false;
            }
        }

        private bool SingleListAddOrEdit<T>(IReadOnlyList<T> listToEdit, TwoName name, DailyValuation oldData, DailyValuation data, IReportLogger reportLogger = null) where T : IValueList
        {
            for (int accountIndex = 0; accountIndex < listToEdit.Count; accountIndex++)
            {
                if (listToEdit[accountIndex].Names.IsEqualTo(name))
                {
                    // now edit data
                    return listToEdit[accountIndex].TryEditData(oldData.Day, data.Day, data.Value, reportLogger);
                }
            }

            return false;
        }
    }
}
