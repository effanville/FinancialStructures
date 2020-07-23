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
        public bool TryEditName(AccountType elementType, NameData oldName, NameData newName, IReportLogger reportLogger = null)
        {
            switch (elementType)
            {
                case (AccountType.Security):
                {
                    for (int fundIndex = 0; fundIndex < NumberOf(AccountType.Security); fundIndex++)
                    {
                        if (oldName.IsEqualTo(Funds[fundIndex].Names))
                        {
                            OnPortfolioChanged(Funds[fundIndex], new EventArgs());
                            return Funds[fundIndex].EditNameData(newName);
                        }
                    }

                    _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.EditingData, $"Renaming {elementType}: Could not find {elementType} with name {oldName}.");
                    return false;
                }
                case (AccountType.Currency):
                {
                    return TryEditNameSingleList(Currencies, elementType, oldName, newName, reportLogger);
                }
                case (AccountType.BankAccount):
                {
                    return TryEditNameSingleList(BankAccounts, elementType, oldName, newName, reportLogger);
                }
                case (AccountType.Benchmark):
                {
                    return TryEditNameSingleList(BenchMarks, elementType, oldName, newName, reportLogger);
                }
                default:
                {
                    _ = reportLogger.LogUseful(ReportType.Error, ReportLocation.EditingData, $"Editing an Unknown type.");
                    return false;
                }
            }
        }

        private bool TryEditNameSingleList<T>(List<T> values, AccountType elementType, NameData oldName, NameData newName, IReportLogger reportLogger = null) where T : ISingleValueDataList
        {
            for (int AccountIndex = 0; AccountIndex < values.Count; AccountIndex++)
            {
                if (values[AccountIndex].Names.IsEqualTo(oldName))
                {
                    OnPortfolioChanged(values[AccountIndex], new EventArgs());
                    return values[AccountIndex].EditNameData(newName);
                }
            }

            _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.EditingData, $"Renaming {elementType}: Could not find {elementType} with name {oldName}.");
            return false;
        }
    }
}
