using System;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.FinanceStructures.Implementation;
using Effanville.FinancialStructures.FinanceStructures.Implementation.Asset;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.FinanceStructures
{
    /// <summary>
    /// Provides factory methods for the <see cref="IValueList"/> interface implementations.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public sealed class IValueListFactory<TImplementation>
    {
        private readonly Func<Account, NameData, TImplementation> _constructor;
        
        public IValueListFactory()
        {
        }
        
        public IValueListFactory(Func<Account, NameData, TImplementation> constructor)
        {
            _constructor = constructor;
        }
        
        /// <summary>
        /// Creates an instance of an IValueList.
        /// </summary>
        /// <param name="account">The type of ValueList to create.</param>
        public IValueList Create(Account account)
        {
            switch (account)
            {
                case Account.Security:
                case Account.Pension:
                    return new Security(account);
                case Account.Benchmark:
                    return new Sector();
                case Account.BankAccount:
                    return new CashAccount();
                case Account.Currency:
                    return new Currency();
                case Account.Asset:
                    return new AmortisableAsset();
                case Account.All:
                case Account.Unknown:
                default:
                    return null;
            }
        }
        
        /// <summary>
        /// Creates an instance of an IValueList.
        /// </summary>
        /// <param name="account">The type of ValueList to create.</param>
        /// <param name="data"></param>
        public TImplementation Create(Account account, NameData data)
        {
            if (_constructor != null)
                return _constructor(account, data);
            
            switch (account)
            {
                case Account.Security:
                case Account.Pension:
                {
                    Security retVal = new Security(account, data);
                    if (retVal is TImplementation impl)
                    {
                        return impl;
                    }

                    return default;
                }
                case Account.Benchmark:
                {
                    Sector retVal = new Sector(data);
                    if (retVal is TImplementation impl)
                    {
                        return impl;
                    }

                    return default;
                }
                case Account.BankAccount:
                {
                    CashAccount retVal = new CashAccount(data);
                    if (retVal is TImplementation impl)
                    {
                        return impl;
                    }

                    return default;
                }
                case Account.Currency:
                {
                    Currency retVal = new Currency(data);
                    if (retVal is TImplementation impl)
                    {
                        return impl;
                    }

                    return default;
                }
                case Account.Asset:
                {
                    AmortisableAsset retVal = new AmortisableAsset(data);
                    if (retVal is TImplementation impl)
                    {
                        return impl;
                    }

                    return default;
                }
                case Account.All:
                case Account.Unknown:
                default:
                    return default;
            }
        }
    }
}
