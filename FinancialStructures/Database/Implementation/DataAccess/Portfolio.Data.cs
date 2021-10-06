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
        public IReadOnlyList<SecurityDayData> SecurityData(TwoName name, IReportLogger reportLogger = null)
        {
            foreach (ISecurity security in FundsThreadSafe)
            {
                if (name.IsEqualTo(security.Names))
                {
                    return security.GetDataForDisplay();
                }
            }

            _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.DatabaseAccess, $"Could not find {Account.Security} - {name}");
            return new List<SecurityDayData>();
        }

        /// <inheritdoc/>
        public IReadOnlyList<DailyValuation> NumberData(Account elementType, TwoName name, IReportLogger reportLogger = null)
        {
            switch (elementType)
            {
                case Account.Security:
                {
                    return SingleDataListDataObtainer(FundsThreadSafe, elementType, name, reportLogger);
                }
                case Account.Currency:
                {
                    return SingleDataListDataObtainer(CurrenciesThreadSafe, elementType, name, reportLogger);
                }
                case Account.BankAccount:
                {
                    return SingleDataListDataObtainer(BankAccountsThreadSafe, elementType, name, reportLogger);
                }
                case Account.Benchmark:
                {
                    return SingleDataListDataObtainer(BenchMarksThreadSafe, elementType, name, reportLogger);
                }
                case Account.All:
                default:
                {
                    return new List<DailyValuation>();
                }
            }
        }

        private static List<DailyValuation> SingleDataListDataObtainer<T>(IReadOnlyList<T> objects, Account elementType, TwoName name, IReportLogger reportLogger = null) where T : IValueList
        {
            foreach (T account in objects)
            {
                if (name.IsEqualTo(account.Names))
                {
                    return account.ListOfValues();
                }
            }

            _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.DatabaseAccess, $"Could not find {elementType} - {name}");
            return new List<DailyValuation>();
        }
    }
}
