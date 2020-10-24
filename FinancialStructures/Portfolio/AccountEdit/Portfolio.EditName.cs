using System.Collections.Generic;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using StructureCommon.Reporting;

namespace FinancialStructures.Database
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
                    for (int fundIndex = 0; fundIndex < NumberOf(Account.Security); fundIndex++)
                    {
                        if (oldName.IsEqualTo(Funds[fundIndex].Names))
                        {
                            OnPortfolioChanged(Funds[fundIndex], new PortfolioEventArgs(Account.Security));
                            return Funds[fundIndex].EditNameData(newName);
                        }
                    }

                    _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.EditingData, $"Renaming {elementType}: Could not find {elementType} with name {oldName}.");
                    return false;
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

        private bool TryEditNameSingleList<T>(List<T> values, Account elementType, NameData oldName, NameData newName, IReportLogger reportLogger = null) where T : ISingleValueDataList
        {
            for (int AccountIndex = 0; AccountIndex < values.Count; AccountIndex++)
            {
                if (values[AccountIndex].Names.IsEqualTo(oldName))
                {
                    OnPortfolioChanged(values[AccountIndex], new PortfolioEventArgs(elementType));
                    return values[AccountIndex].EditNameData(newName);
                }
            }

            _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.EditingData, $"Renaming {elementType}: Could not find {elementType} with name {oldName}.");
            return false;
        }
    }
}
