using System.Collections.Generic;

using Common.Structure.Reporting;

using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public bool TryEditName(Account elementType, NameData oldName, NameData newName, IReportLogger reportLogger = null)
        {
            switch (elementType)
            {
                case Account.Security:
                {
                    return TryEditNameSingleList(FundsThreadSafe, elementType, oldName, newName, reportLogger);
                }
                case Account.Currency:
                {
                    return TryEditNameSingleList(CurrenciesThreadSafe, elementType, oldName, newName, reportLogger);
                }
                case Account.BankAccount:
                {
                    return TryEditNameSingleList(BankAccountsThreadSafe, elementType, oldName, newName, reportLogger);
                }
                case Account.Benchmark:
                {
                    return TryEditNameSingleList(BenchMarksThreadSafe, elementType, oldName, newName, reportLogger);
                }
                case Account.Asset:
                {
                    return TryEditNameSingleList(Assets, elementType, oldName, newName, reportLogger);
                }
                default:
                {
                    _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.EditingData, $"Editing an Unknown type.");
                    return false;
                }
            }
        }

        private static bool TryEditNameSingleList<T>(IReadOnlyList<T> values, Account elementType, NameData oldName, NameData newName, IReportLogger reportLogger = null) where T : IValueList
        {
            for (int AccountIndex = 0; AccountIndex < values.Count; AccountIndex++)
            {
                if (values[AccountIndex].Names.IsEqualTo(oldName))
                {
                    return values[AccountIndex].EditNameData(newName);
                }
            }

            _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.EditingData, $"Renaming {elementType}: Could not find {elementType} with name {oldName}.");
            return false;
        }
    }
}
