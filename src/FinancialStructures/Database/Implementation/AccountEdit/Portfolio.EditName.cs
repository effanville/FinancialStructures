using Effanville.Common.Structure.DataEdit;
using Effanville.FinancialStructures.Database.Extensions;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Implementation;

public partial class Portfolio
{
    /// <inheritdoc/>
    public UpdateResult<(Account, NameData)> TryEditName(Account elementType, NameData oldName, NameData newName)
    {
        TwoName oldTwoName = oldName.ToTwoName();
        UpdateResult<NameData> outcome = this.TryPerformEdit<IValueList, NameData>(
            elementType,
            oldTwoName,
            valueList => valueList.EditNameData(newName));

        TwoName newTwoName = newName.ToTwoName();

        bool keyUpdate = elementType switch
        {
            Account.Security => _funds.TryUpdateKey(oldTwoName, newTwoName),
            Account.Benchmark => _benchmarks.TryUpdateKey(oldTwoName, newTwoName),
            Account.BankAccount => _bankAccounts.TryUpdateKey(oldTwoName, newTwoName),
            Account.Currency => _currencies.TryUpdateKey(oldTwoName, newTwoName),
            Account.Asset => _assets.TryUpdateKey(oldTwoName, newTwoName),
            Account.Pension => _pensions.TryUpdateKey(oldTwoName, newTwoName),
            _ => false
        };

        bool needsNewKey = newTwoName.IsEqualTo(oldTwoName);
        return new UpdateResult<(Account, NameData)>
        {
            Success = outcome.Success && (needsNewKey ? true : keyUpdate),
            IsAdd = outcome.IsAdd,
            IsChange = outcome.IsChange,
            IsDelete = outcome.IsDelete,
            OldValue = (elementType, outcome.OldValue),
            NewValue = (elementType, outcome.NewValue),
            Message = outcome.Message,
        };
    }
}
