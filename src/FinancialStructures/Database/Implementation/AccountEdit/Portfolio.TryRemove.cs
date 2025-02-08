using Effanville.Common.Structure.ChangeLogging;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Implementation;

public partial class Portfolio
{
    /// <inheritdoc/>
    public UpdateResult<(Account, NameData)> TryRemove(Account accountType, TwoName name)
    {
        if (string.IsNullOrEmpty(name.Name) && string.IsNullOrEmpty(name.Company))
        {
            return UpdateResult.Fail((accountType, name.ToNameData()), $"Company `{name.Company}' and name `{name.Name}' cannot both be empty", isDelete: true);
        }

        switch (accountType)
        {
            case Account.Security:
            {
                return _funds.Remove(name);
            }
            case Account.Currency:
            {
                return _currencies.Remove(name);
            }
            case Account.BankAccount:
            {
                return _bankAccounts.Remove(name);
            }
            case Account.Benchmark:
            {
                return _benchmarks.Remove(name);
            }
            case Account.Asset:
            {
                return _assets.Remove(name);
            }
            case Account.Pension:
            {
                return _pensions.Remove(name);
            }
            case Account.Unknown:
            case Account.All:
            default:
                return UpdateResult.Fail((accountType, name.ToNameData()), "Editing an Unknown type.", isDelete: true);
        }
    }
}