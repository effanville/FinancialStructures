using FinancialStructures.DataStructures;
using FinancialStructures.NamingStructures;
using StructureCommon.Reporting;
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

        bool TryAddData(DateTime date, double unitPrice, double shares, double investment, IReportLogger reportLogger);
        void UpdateSecurityData(DateTime day, double value, IReportLogger reportLogger = null);
        bool TryEditData(DateTime oldDate, DateTime newDate, double shares, double unitPrice, double Investment, IReportLogger reportLogger = null);
        bool EditNameData(NameData name);
        bool IsSectorLinked(string sectorName);
        bool TryDeleteData(DateTime date, IReportLogger reportLogger = null);
    }
}
