using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.Database.Extensions;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Implementation;

public partial class Portfolio
{
    /// <inheritdoc/>
    public bool TryEditName(Account elementType, NameData oldName, NameData newName, IReportLogger reportLogger = null)
    {
        TwoName oldTwoName = oldName.ToTwoName();
        bool outcome = this.TryPerformEdit(elementType,
            oldTwoName,
            valueList => valueList.EditNameData(newName),
            ReportLocation.EditingData,
            reportLogger);

        TwoName newTwoName = newName.ToTwoName();
        return outcome
           && elementType switch
           {
               Account.Security => _funds.TryUpdateKey(oldTwoName, newTwoName),
               Account.Benchmark => _benchmarks.TryUpdateKey(oldTwoName, newTwoName),
               Account.BankAccount => _bankAccounts.TryUpdateKey(oldTwoName, newTwoName),
               Account.Currency => _currencies.TryUpdateKey(oldTwoName, newTwoName),
               Account.Asset => _assets.TryUpdateKey(oldTwoName, newTwoName),
               Account.Pension => _pensions.TryUpdateKey(oldTwoName, newTwoName),
               _ => false
           };
    }
}
