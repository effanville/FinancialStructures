using System;
using StructureCommon.DataStructures;

namespace FinancialStructures.FinanceInterfaces
{
    public interface ICashAccount : ISingleValueDataList
    {
        new ICashAccount Copy();
        DailyValuation Value(DateTime date, ICurrency currency = null);
        DailyValuation LatestValue(ICurrency currency = null);
        DailyValuation FirstValue(ICurrency currency = null);
        DailyValuation NearestEarlierValuation(DateTime date, ICurrency currency = null);
        DailyValuation LastEarlierValuation(DateTime date, ICurrency currency = null);
    }
}
