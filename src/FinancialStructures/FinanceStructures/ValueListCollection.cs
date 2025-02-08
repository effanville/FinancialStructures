using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Effanville.Common.Structure.ChangeLogging;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.FinanceStructures.Implementation;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.FinanceStructures;

public class ValueListCollection<TInterface, TImplementation>
    where TInterface : IReadOnlyValueList
    where TImplementation : ValueList, TInterface
{
    private readonly Account _account;
    private readonly IValueListFactory<TImplementation> _factory;
    private readonly ReaderWriterLockSlim _collectionLock = new ReaderWriterLockSlim();

    private Dictionary<TwoName, TImplementation> _collectionDictionary =
        new Dictionary<TwoName, TImplementation>();

    public EventHandler<PortfolioEventArgs> CollectionChanged;
    public EventHandler<PortfolioEventArgs> CollectionItemChanged;

    public ValueListCollection(Account account, Func<Account, NameData, TImplementation> constructor)
    {
        _account = account;
        _factory = new IValueListFactory<TImplementation>(constructor);
    }

    public ValueListCollection(Account account, IValueListFactory<TImplementation> factory)
    {
        _account = account;
        _factory = factory;
    }

    public IReadOnlyList<TInterface> Values
    {
        get
        {
            _collectionLock.EnterReadLock();
            try
            {
                return _collectionDictionary.Values.ToList();
            }
            finally
            {
                _collectionLock.ExitReadLock();
            }
        }
    }

    public bool TryGetAndCast<TCastedType>(
        TwoName names,
        out TCastedType valueList)
    {
        valueList = default;
        _collectionLock.EnterReadLock();
        try
        {
            TwoName searchName = new TwoName(names.Company, names.Name);
            if (!_collectionDictionary.TryGetValue(searchName, out TImplementation account))
            {
                return false;
            }

            if (account is not TCastedType castedObject)
            {
                return false;
            }

            valueList = castedObject;
            return true;

        }
        finally
        {
            _collectionLock.ExitReadLock();
        }
    }

    public UpdateResult<(Account, NameData)> TryAdd(Account accountType, NameData name)
    {
        if (_account != accountType && _account != Account.All)
        {
            return UpdateResult.Fail((accountType, name), $"{accountType}-{name} attempted to be added to type {_account}.");

        }

        bool result = false;
        _collectionLock.EnterWriteLock();
        try
        {
            TImplementation newObject = _factory.Create(accountType, name);
            if (newObject == null)
            {
                return UpdateResult.Fail((accountType, name), $"{accountType}-{name} Could not create.", isAdd: true);

            }

            if (_collectionDictionary.TryAdd(newObject.Names.ToTwoName(), newObject))
            {
                result = true;
                newObject.DataEdit += OnCollectionItemChanged;
            }
        }
        finally
        {
            _collectionLock.ExitWriteLock();
        }

        if (result)
        {
            OnCollectionChanged(this, new PortfolioEventArgs(_account));
        }

        return UpdateResult.Add((accountType, name));
    }

    public bool TryUpdateKey(TwoName oldKey, TwoName newKey)
    {
        if (oldKey.IsEqualTo(newKey))
        {
            return false;
        }

        _collectionLock.EnterWriteLock();
        try
        {
            TwoName oldKeyValue = oldKey.ToTwoName();
            if (!_collectionDictionary.TryGetValue(oldKeyValue, out TImplementation valueList))
            {
                return false;
            }

            _ = _collectionDictionary.Remove(oldKeyValue);
            _collectionDictionary.Add(newKey.ToTwoName(), valueList);
        }
        finally
        {
            _collectionLock.ExitWriteLock();
        }

        return true;
    }

    internal void AddValueList(TImplementation security)
    {
        _collectionLock.EnterWriteLock();
        try
        {
            _collectionDictionary.Add(security.Names.ToTwoName(), security);
        }
        finally
        {
            _collectionLock.ExitWriteLock();
        }
    }

    public UpdateResult<(Account, NameData)> Remove(TwoName name)
    {
        _collectionLock.EnterWriteLock();
        try
        {
            TwoName nameToRemove = new TwoName(name.Company, name.Name);
            if (_collectionDictionary.TryGetValue(nameToRemove, out TImplementation list))
            {
                list.DataEdit -= OnCollectionItemChanged;
            }

            if (_collectionDictionary.Remove(nameToRemove))

            {


                OnCollectionChanged(this, new PortfolioEventArgs(_account));
                return UpdateResult.Delete((_account, name.ToNameData()));
            }
        }
        finally
        {
            _collectionLock.ExitWriteLock();
        }

        return UpdateResult.Fail((_account, name.ToNameData()), $"{_account}-{name} could not be found in the database.", isDelete: true);
    }

    public bool Exists(TwoName name)
    {
        _collectionLock.EnterReadLock();
        try
        {
            return _collectionDictionary.ContainsKey(name);
        }
        finally
        {
            _collectionLock.ExitReadLock();
        }
    }

    public void CopyFrom(ValueListCollection<TInterface, TImplementation> values)
    {
        _collectionLock.EnterWriteLock();
        try
        {
            foreach (KeyValuePair<TwoName, TImplementation> security in values._collectionDictionary)
            {
                _collectionDictionary.Add(security.Key, (TImplementation)security.Value.Copy());
            }
        }
        finally
        {
            _collectionLock.ExitWriteLock();
        }
    }

    public void ReplaceDictionary(ValueListCollection<TInterface, TImplementation> values)
    {
        _collectionLock.EnterWriteLock();
        try
        {
            _collectionDictionary = values._collectionDictionary;
        }
        finally
        {
            _collectionLock.ExitWriteLock();
        }
    }

    public void SetupCollectionChangedEvents()
    {
        _collectionLock.EnterWriteLock();
        try
        {
            foreach (TImplementation security in _collectionDictionary.Values)
            {
                security.DataEdit += OnCollectionItemChanged;
                security.SetupEventListening();
            }
        }
        finally
        {
            _collectionLock.ExitWriteLock();
        }
    }

    private void OnCollectionChanged(object obj, PortfolioEventArgs e)
    {
        EventHandler<PortfolioEventArgs> handler = CollectionChanged;
        handler?.Invoke(obj, e);
    }

    private void OnCollectionItemChanged(object obj, PortfolioEventArgs e)
    {
        EventHandler<PortfolioEventArgs> handler = CollectionItemChanged;
        handler?.Invoke(obj, e);
    }
}