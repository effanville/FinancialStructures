using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public bool TryGetAccount(Account accountType, TwoName names, out IValueList valueList)
        {
            valueList = null;
            switch (accountType)
            {
                case Account.Security:
                {
                    lock (_fundsLock)
                    {
                        var searchName = new TwoName(names.Company, names.Name);
                        if (!_fundsDictionary.TryGetValue(searchName, out var security))
                        {
                            return false;
                        }

                        valueList = security;
                        return true;
                    }
                }
                case Account.BankAccount:
                {
                    lock (_bankAccountsLock)
                    {
                        var searchName = new TwoName(names.Company, names.Name);
                        if (!_bankAccountsDictionary.TryGetValue(searchName, out var bankAccount))
                        {
                            return false;
                        }
                        
                        valueList = bankAccount;
                        return true;
                    }
                }
                case Account.Currency:
                {
                    lock (_currenciesLock)
                    {
                        var searchName = new TwoName(names.Company, names.Name);
                        if (!_currenciesDictionary.TryGetValue(searchName, out var bankAccount))
                        {
                            return false;
                        }
                        
                        valueList = bankAccount;
                        return true;
                    }
                }
                case Account.Benchmark:
                {
                    lock (_benchmarksLock)
                    {
                        var searchName = new TwoName(names.Company, names.Name);
                        if (!_benchMarksDictionary.TryGetValue(searchName, out var bankAccount))
                        {
                            return false;
                        }
                        
                        valueList = bankAccount;
                        return true;
                    }
                }
                case Account.Asset:
                {
                    lock (_assetsLock)
                    {
                        var searchName = new TwoName(names.Company, names.Name);
                        if (!_assetsDictionary.TryGetValue(searchName, out var bankAccount))
                        {
                            return false;
                        }
                        
                        valueList = bankAccount;
                        return true;
                    }
                }
                case Account.Pension:
                {
                    lock (_pensionsLock)
                    {
                        var searchName = new TwoName(names.Company, names.Name);
                        if (!_pensionsDictionary.TryGetValue(searchName, out var bankAccount))
                        {
                            return false;
                        }
                        
                        valueList = bankAccount;
                        return true;
                    }
                }
                default:
                case Account.All:
                case Account.Unknown:
                {
                    valueList = null;
                    return false;
                }
            }
        }
    }
}