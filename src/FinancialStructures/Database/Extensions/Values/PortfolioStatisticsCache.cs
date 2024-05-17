using System;
using System.Collections.Generic;

using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Extensions.Values
{
    public class PortfolioStatisticsCache : IPortfolioStatisticsCache
    {
        private readonly Dictionary<(Account, TwoName, DateTime, string), object> _accountStatCache = new ();
        private readonly Dictionary<(Totals, TwoName, DateTime, string), object> _totalsStatCache = new ();
        public bool AddValue(Totals totals, TwoName name, DateTime date, string statistic, object value) 
            => _totalsStatCache.TryAdd((totals, name, date, statistic), value);

        public bool TryGetValue(Totals totals, TwoName name, DateTime date, string statistic, out object value) 
            => _totalsStatCache.TryGetValue((totals, name, date, statistic), out value);

        public bool AddValue(Account account, TwoName name, DateTime date, string statistic, object value) 
            => _accountStatCache.TryAdd((account, name, date, statistic), value);

        public bool TryGetValue(Account account, TwoName name, DateTime date, string statistic, out object value) 
            => _accountStatCache.TryGetValue((account, name, date, statistic), out value);

        public void Clear()
        { 
            _accountStatCache.Clear();
            _totalsStatCache.Clear();
        }
    }
}