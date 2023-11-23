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
                    _fundsLock.EnterReadLock();
                    try
                    {
                        var searchName = new TwoName(names.Company, names.Name);
                        if (!_fundsDictionary.TryGetValue(searchName, out var security))
                        {
                            return false;
                        }

                        valueList = security;
                        return true;
                    }
                    finally
                    {
                        _fundsLock.ExitReadLock();
                    }
                }
                case Account.BankAccount:
                {
                    _bankAccountsLock.EnterReadLock();
                    try
                    {
                        var searchName = new TwoName(names.Company, names.Name);
                        if (!_bankAccountsDictionary.TryGetValue(searchName, out var bankAccount))
                        {
                            return false;
                        }

                        valueList = bankAccount;
                        return true;
                    }
                    finally
                    {
                        _bankAccountsLock.ExitReadLock();
                    }
                }
                case Account.Currency:
                {
                    _currenciesLock.EnterReadLock();
                    try
                    {
                        var searchName = new TwoName(names.Company, names.Name);
                        if (!_currenciesDictionary.TryGetValue(searchName, out var bankAccount))
                        {
                            return false;
                        }

                        valueList = bankAccount;
                        return true;
                    }
                    finally
                    {
                        _currenciesLock.ExitReadLock();
                    }
                }
                case Account.Benchmark:
                {
                    _benchmarksLock.EnterReadLock();
                    try
                    {
                        var searchName = new TwoName(names.Company, names.Name);
                        if (!_benchMarksDictionary.TryGetValue(searchName, out var bankAccount))
                        {
                            return false;
                        }

                        valueList = bankAccount;
                        return true;
                    }
                    finally
                    {
                        _benchmarksLock.ExitReadLock();
                    }
                }
                case Account.Asset:
                {
                    _assetsLock.EnterReadLock();
                    try
                    {
                        var searchName = new TwoName(names.Company, names.Name);
                        if (!_assetsDictionary.TryGetValue(searchName, out var bankAccount))
                        {
                            return false;
                        }

                        valueList = bankAccount;
                        return true;
                    }
                    finally
                    {
                        _assetsLock.ExitReadLock();
                    }
                }
                case Account.Pension:
                {
                    _pensionsLock.EnterReadLock();
                    try
                    {
                        var searchName = new TwoName(names.Company, names.Name);
                        if (!_pensionsDictionary.TryGetValue(searchName, out var bankAccount))
                        {
                            return false;
                        }

                        valueList = bankAccount;
                        return true;
                    }
                    finally
                    {
                        _pensionsLock.ExitReadLock();
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