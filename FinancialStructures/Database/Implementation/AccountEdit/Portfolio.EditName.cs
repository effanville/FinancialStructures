using System.Collections.Generic;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;
using StructureCommon.Reporting;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public bool TryEditName(Account elementType, NameData oldName, NameData newName, IReportLogger reportLogger = null)
        {
            switch (elementType)
            {
                case (Account.Security):
                {
                    return TryEditNameSingleList(Funds, elementType, oldName, newName, reportLogger);
                }
                case (Account.Currency):
                {
                    return TryEditNameSingleList(Currencies, elementType, oldName, newName, reportLogger);
                }
                case (Account.BankAccount):
                {
                    return TryEditNameSingleList(BankAccounts, elementType, oldName, newName, reportLogger);
                }
                case (Account.Benchmark):
                {
                    return TryEditNameSingleList(BenchMarks, elementType, oldName, newName, reportLogger);
                }
                default:
                {
                    _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.EditingData, $"Editing an Unknown type.");
                    return false;
                }
            }
        }

        private bool TryEditNameSingleList<T>(List<T> values, Account elementType, NameData oldName, NameData newName, IReportLogger reportLogger = null) where T : IValueList
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
