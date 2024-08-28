using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.FinanceStructures.Implementation;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.FinanceStructures
{
    public class ValueListCollection<TInterface, TImplementation>
        where TInterface : IReadOnlyValueList
        where TImplementation : ValueList, TInterface
    {
        private readonly Account _account;
        private readonly Func<Account, NameData, TImplementation> _constructor;
        private readonly ReaderWriterLockSlim _collectionLock = new ReaderWriterLockSlim();

        private Dictionary<TwoName, TImplementation> _collectionDictionary =
            new Dictionary<TwoName, TImplementation>();

        public EventHandler<PortfolioEventArgs> CollectionChanged;
        public EventHandler<PortfolioEventArgs> CollectionItemChanged;

        public ValueListCollection(Account account, Func<Account, NameData, TImplementation> constructor)
        {
            _account = account;
            _constructor = constructor;
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

        public bool TryAdd(Account accountType, NameData name, IReportLogger reportLogger = null)
        {
            if (_account != accountType)
            {
                reportLogger?.Error(ReportLocation.AddingData.ToString(), $"{accountType}-{name} attempted to be added to type {_account}.");
                return false;
            }
            bool result = false;
            _collectionLock.EnterWriteLock();
            try
            {
                TImplementation newObject = _constructor(accountType, name);
                if (newObject == null)
                {
                    reportLogger?.Error(ReportLocation.AddingData.ToString(), $"{accountType}-{name} Could not create.");
                    return false;
                }

                if (_collectionDictionary.TryAdd(newObject.Names.ToTwoName(), newObject))
                {
                    result = true;
                    newObject.DataEdit += OnCollectionItemChanged;
                    reportLogger?.Log(ReportSeverity.Detailed, ReportType.Information, 
                        ReportLocation.AddingData.ToString(), $"{accountType}-{name} added to database.");
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
            return result;
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

        public bool Remove(TwoName name, IReportLogger reportLogger = null)
        {
            _collectionLock.EnterWriteLock();
            try
            {
                TwoName nameToRemove = new TwoName(name.Company, name.Name);
                if (_collectionDictionary.TryGetValue(nameToRemove, out TImplementation list))
                {
                    list.DataEdit -= OnCollectionItemChanged;
                }

                bool removed = _collectionDictionary.Remove(nameToRemove);
                if (removed)
                {
                    reportLogger?.Log(ReportSeverity.Detailed, ReportType.Information,
                        ReportLocation.DeletingData.ToString(), $"{_account}-{name} removed from the database.");
                    OnCollectionChanged(this, new PortfolioEventArgs(_account));
                    return true;
                }
            }
            finally
            {
                _collectionLock.ExitWriteLock();
            }

            reportLogger?.Log(ReportType.Error, ReportLocation.DeletingData.ToString(),
                $"{_account}-{name} could not be found in the database.");
            return false;
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
}