using System;

using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Extensions.Values
{
    public interface IPortfolioStatisticsCache
    {
        bool AddValue(Totals totals, TwoName name, DateTime date, string statistic, object value);
        bool TryGetValue(Totals totals, TwoName name, DateTime date, string statistic, out object value);

        bool AddValue(Account account, TwoName name, DateTime date, string statistic, object value);
        bool TryGetValue(Account account, TwoName name, DateTime date, string statistic, out object value);
        void Clear();
    }
}