using System.Collections.Generic;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;
using StructureCommon.Reporting;

namespace FinancialStructures.Database
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public List<SecurityDayData> SecurityData(TwoName name, IReportLogger reportLogger = null)
        {
            foreach (ISecurity security in Funds)
            {
                if (name.IsEqualTo(security.Names))
                {
                    return security.GetDataForDisplay();
                }
            }

            _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.DatabaseAccess, $"Could not find {AccountType.Security} - {name.ToString()}");
            return new List<SecurityDayData>();
        }

        /// <inheritdoc/>
        public List<DailyValuation> NumberData(AccountType elementType, TwoName name, IReportLogger reportLogger = null)
        {
            switch (elementType)
            {
                case (AccountType.Currency):
                {
                    return SingleDataListDataObtainer(Currencies, elementType, name, reportLogger);
                }
                case (AccountType.BankAccount):
                {
                    return SingleDataListDataObtainer(BankAccounts, elementType, name, reportLogger);
                }
                case (AccountType.Benchmark):
                {
                    return SingleDataListDataObtainer(BenchMarks, elementType, name, reportLogger);
                }
                default:
                case (AccountType.Security):
                {
                    return new List<DailyValuation>();
                }
            }
        }

        private List<DailyValuation> SingleDataListDataObtainer<T>(List<T> objects, AccountType elementType, TwoName name, IReportLogger reportLogger = null) where T : ISingleValueDataList
        {
            foreach (T account in objects)
            {
                if (name.IsEqualTo(account.Names))
                {
                    return account.GetDataForDisplay();
                }
            }

            _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.DatabaseAccess, $"Could not find {elementType.ToString()} - {name.ToString()}");
            return new List<DailyValuation>();
        }
    }
}
