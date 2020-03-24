using FinancialStructures.DataStructures;
using FinancialStructures.NamingStructures;
using FinancialStructures.ReportLogging;
using System;
using System.Collections.Generic;

namespace FinancialStructures.FinanceInterfaces
{
    public interface ISingleValueDataList
    {
        string ToString();
        int CompareTo(object obj);
        bool IsEqualTo(ISingleValueDataList otherAccount);
        NameData Names { get; }
        string Name { get; }
        string Company { get; }
        string Url { get; }
        string Currency { get; }
        TimeList Values { get; }

        ISingleValueDataList Copy();
        bool Any();
        int Count();
        DailyValuation LatestValue();
        double CAR(DateTime earlierTime, DateTime laterTime);

        DailyValuation Value(DateTime date);
        List<DayValue_ChangeLogged> GetDataForDisplay();
        bool EditNameData(NameData newNames);
        bool TryAddData(DateTime date, double value, LogReporter reportLogger);
        bool TryEditData(DateTime oldDate, DateTime date, double value, LogReporter reportLogger);
        bool TryDeleteData(DateTime date, LogReporter reportLogger);
        bool TryRemoveSector(string sectorName);
    }
}
