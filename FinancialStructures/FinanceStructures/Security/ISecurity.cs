using FinancialStructures.DataStructures;
using FinancialStructures.NamingStructures;
using FinancialStructures.ReportLogging;
using System;
using System.Collections.Generic;

namespace FinancialStructures.FinanceInterfaces
{
    public interface ISecurity
    {
        NameData Names { get; }
        string Name { get; }
        string Company { get; }
        string Url { get; }
        string Currency { get; }
        HashSet<string> Sectors { get; }
        TimeList Shares { get; }
        TimeList UnitPrice { get; }
        TimeList Investments { get; }
        bool IsEqualTo(ISecurity otherSecurity);
        bool SameName(TwoName otherNames);
        bool SameName(string company, string name);
        int Count();
        bool Any();
        ISecurity Copy();
        List<SecurityDayData> GetDataForDisplay();
        int NumberSectors();

        double TotalInvestment(ICurrency currency = null);
        DailyValuation LatestValue(ICurrency currency = null);
        DailyValuation FirstValue(ICurrency currency = null);
        DailyValuation Value(DateTime date, ICurrency currency = null);
        DailyValuation LastEarlierValuation(DateTime date, ICurrency currency = null);
        DailyValuation NearestEarlierValuation(DateTime date, ICurrency currency = null);
        List<DailyValuation> InvestmentsBetween(DateTime earlierDate, DateTime laterDate, ICurrency currency = null);
        List<DayValue_Named> AllInvestmentsNamed(ICurrency currency = null);
        double IRRTime(DateTime earlierDate, DateTime laterDate, ICurrency currency = null);
        double IRR(ICurrency currency = null);

        bool TryAddData(LogReporter reportLogger, DateTime date, double unitPrice, double shares = 0, double investment = 0);
        void UpdateSecurityData(double value, LogReporter reportLogger, DateTime day);
        bool TryEditData(LogReporter reportLogger, DateTime oldDate, DateTime newDate, double shares, double unitPrice, double Investment);
        bool EditNameData(NameData name);
        bool IsSectorLinked(string sectorName);
        bool TryDeleteData(DateTime date, LogReporter reportLogger);
    }
}
