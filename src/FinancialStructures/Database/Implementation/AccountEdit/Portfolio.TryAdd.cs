using Effanville.Common.Structure.ChangeLogging;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Implementation;

public partial class Portfolio
{
    /// <inheritdoc/>
    public UpdateResult<(Account, NameData)> TryAdd(Account accountType, NameData name)
    {
        if (string.IsNullOrWhiteSpace(name.Name) && string.IsNullOrWhiteSpace(name.Company))
        {
            return UpdateResult.Fail((accountType, name), $"Adding {accountType}: Company '{name.Company}' and name '{name.Name}' cannot both be empty.");
        }

        if (Exists(accountType, name.ToTwoName()))
        {
            OnPortfolioChanged(null, new PortfolioEventArgs(accountType));
            return UpdateResult.Fail((accountType, name), $"{accountType}-{name} already exists.");
        }

        switch (accountType)
        {
            case Account.Security:
            {
                return _funds.TryAdd(accountType, name);
            }
            case Account.Currency:
            {
                if (string.IsNullOrEmpty(name.Company))
                {
                    name.Company = "GBP";
                }

                return _currencies.TryAdd(accountType, name);
            }
            case Account.BankAccount:
            {
                return _bankAccounts.TryAdd(accountType, name);
            }
            case Account.Benchmark:
            {
                return _benchmarks.TryAdd(accountType, name);
            }
            case Account.Asset:
            {
                return _assets.TryAdd(accountType, name);
            }
            case Account.Pension:
            {
                return _pensions.TryAdd(accountType, name);
            }
            case Account.Unknown:
            case Account.All:
            default:
                return UpdateResult.Fail((accountType, name), "Adding an Unknown type to portfolio.");
        }
    }
}
