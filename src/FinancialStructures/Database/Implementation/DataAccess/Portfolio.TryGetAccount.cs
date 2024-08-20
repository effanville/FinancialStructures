using System.Collections.Generic;
using System.Threading;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public bool TryGetAccount<TNamedFinancialObject>(
            Account accountType,
            TwoName names,
            out TNamedFinancialObject valueList)
            where TNamedFinancialObject : IReadOnlyNamedFinancialObject
        {
            valueList = default;
            switch (accountType)
            {
                case Account.Security:
                {
                    return GetAccountAndCast(_fundsDictionary, _fundsLock, names, out valueList);
                }
                case Account.BankAccount:
                {
                    return GetAccountAndCast(_bankAccountsDictionary, _bankAccountsLock, names, out valueList);
                }
                case Account.Currency:
                {
                    return GetAccountAndCast(_currenciesDictionary, _currenciesLock, names, out valueList);
                }
                case Account.Benchmark:
                {
                    return GetAccountAndCast(_benchMarksDictionary, _benchmarksLock, names, out valueList);
                }
                case Account.Asset:
                {
                    return GetAccountAndCast(_assetsDictionary, _assetsLock, names, out valueList);
                }
                case Account.Pension:
                {
                    return GetAccountAndCast(_pensionsDictionary, _pensionsLock, names, out valueList);
                }
                default:
                case Account.All:
                case Account.Unknown:
                {
                    return false;
                }
            }
        }
        
        private static bool GetAccountAndCast<TDictionaryType, TCastedType>(
            Dictionary<TwoName, TDictionaryType> accountDictionary, 
            ReaderWriterLockSlim accountLock,
            TwoName names,
            out TCastedType valueList)
        {
            valueList = default;
            accountLock.EnterReadLock();
            try
            {
                TwoName searchName = new TwoName(names.Company, names.Name);
                if (!accountDictionary.TryGetValue(searchName, out TDictionaryType account))
                {
                    return false;
                }

                if (account is TCastedType castedObject)
                {
                    valueList = castedObject;
                    return true;
                }

                return false;
            }
            finally
            {
                accountLock.ExitReadLock();
            }
        }
    }
}