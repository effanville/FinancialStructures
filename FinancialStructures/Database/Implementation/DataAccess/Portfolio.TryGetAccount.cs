using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public bool TryGetAccount(Account accountType, TwoName names, out IValueList desired)
        {
            bool success = false;
            desired = null;
            switch (accountType)
            {
                case Account.Security:
                {
                    foreach (ISecurity sec in FundsThreadSafe)
                    {
                        if (names.IsEqualTo(sec.Names))
                        {
                            desired = sec;
                            success = true;
                        }
                    }
                    break;
                }
                case Account.BankAccount:
                {
                    foreach (IExchangableValueList sec in BankAccountsThreadSafe)
                    {
                        if (names.IsEqualTo(sec.Names))
                        {
                            desired = sec;
                            success = true;
                        }
                    }
                    break;
                }
                case Account.Currency:
                {
                    foreach (ICurrency currency in CurrenciesThreadSafe)
                    {
                        if (names.IsEqualTo(currency.Names))
                        {
                            desired = currency;
                            success = true;
                        }
                    }
                    break;
                }
                case Account.Benchmark:
                {
                    foreach (IValueList sector in BenchMarksThreadSafe)
                    {
                        if (sector.Names.Name == names.Name)
                        {
                            desired = sector;
                            success = true;
                        }
                    }
                    break;
                }
                case Account.Asset:
                {
                    foreach (IValueList asset in Assets)
                    {
                        if (names.IsEqualTo(asset.Names))
                        {
                            desired = asset;
                            success = true;
                        }
                    }
                    break;
                }
                case Account.All:
                {
                    desired = null;
                    success = false;
                    break;
                }
            }

            return success;
        }
    }
}
