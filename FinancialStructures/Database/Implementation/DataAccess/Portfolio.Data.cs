using System.Collections.Generic;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;
using StructureCommon.Reporting;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public List<SecurityDayData> SecurityData(TwoName name, IReportLogger reportLogger = null)
        {
            foreach (ISecurity security in FundsThreadSafe)
            {
                if (name.IsEqualTo(security.Names))
                {
                    return security.GetDataForDisplay();
                }
            }

            _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.DatabaseAccess, $"Could not find {Account.Security} - {name.ToString()}");
            return new List<SecurityDayData>();
        }

        /// <inheritdoc/>
        public List<DailyValuation> NumberData(Account elementType, TwoName name, IReportLogger reportLogger = null)
        {
            switch (elementType)
            {
                case (Account.Security):
                {
                    return SingleDataListDataObtainer(FundsThreadSafe, elementType, name, reportLogger);
                }
                case (Account.Currency):
                {
                    return SingleDataListDataObtainer(CurrenciesThreadSafe, elementType, name, reportLogger);
                }
                case (Account.BankAccount):
                {
                    return SingleDataListDataObtainer(BankAccountsThreadSafe, elementType, name, reportLogger);
                }
                case (Account.Benchmark):
                {
                    return SingleDataListDataObtainer(BenchMarksThreadSafe, elementType, name, reportLogger);
                }
                default:
                {
                    return new List<DailyValuation>();
                }
            }
        }

        private List<DailyValuation> SingleDataListDataObtainer<T>(IReadOnlyList<T> objects, Account elementType, TwoName name, IReportLogger reportLogger = null) where T : IValueList
        {
            foreach (T account in objects)
            {
                if (name.IsEqualTo(account.Names))
                {
                    return account.ListOfValues();
                }
            }

            _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.DatabaseAccess, $"Could not find {elementType.ToString()} - {name.ToString()}");
            return new List<DailyValuation>();
        }
    }
}
