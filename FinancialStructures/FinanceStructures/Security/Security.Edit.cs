using FinancialStructures.DataStructures;
using FinancialStructures.NamingStructures;
using StructureCommon.Reporting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// Contains data editing of a security class.
    /// </summary>
    public partial class Security
    {
        /// <summary>
        /// Attempts to add data for the date specified.
        /// If cannot add any value that one wants to, then doesn't add all the values chosen.
        /// </summary>
        public bool TryAddData(DateTime date, double unitPrice, double shares = 0, double investment = 0, IReportLogger reportLogger = null)
        {
            if (DoesDateSharesDataExist(date, out int _) || DoesDateInvestmentDataExist(date, out int _) || DoesDateUnitPriceDataExist(date, out int _))
            {
                reportLogger?.LogUseful(ReportType.Error, ReportLocation.AddingData, $"Security {Names.ToString()} already has NumShares or UnitPrice or Investment data on {date.ToString("d")}.");
                return false;
            }

            return fShares.TryAddValue(date, shares, reportLogger) & fUnitPrice.TryAddValue(date, unitPrice, reportLogger) & fInvestments.TryAddValue(date, investment, reportLogger) && ComputeInvestments(reportLogger);
        }

        public List<object> CreateDataFromCsv(List<string[]> valuationsToRead, IReportLogger reportLogger = null)
        {
            var dailyValuations = new List<object>();
            foreach (var dayValuation in valuationsToRead)
            {
                if (dayValuation.Length != 4)
                {
                    reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Loading, "Line in Csv file has incomplete data.");
                    break;
                }

                var line = new SecurityDayData(DateTime.Parse(dayValuation[0]), double.Parse(dayValuation[1]), double.Parse(dayValuation[2]), double.Parse(dayValuation[3]));
                dailyValuations.Add(line);
            }

            return dailyValuations;
        }

        public void WriteDataToCsv(TextWriter writer, IReportLogger reportLogger)
        {
            foreach (SecurityDayData value in GetDataForDisplay())
            {
                writer.WriteLine(value.ToString());
            }
        }

        /// <summary>
        /// Adds the value to the data with todays date and with the latest number of shares.
        /// </summary>
        public void UpdateSecurityData(DateTime day, double value, IReportLogger reportLogger = null)
        {
            // best approximation for number of units is last known number of units.
            TryGetEarlierData(day, out DailyValuation _, out DailyValuation units, out DailyValuation _);
            if (units == null)
            {
                units = new DailyValuation(day, 0);
            }

            TryAddData(day, value, units.Value, reportLogger: reportLogger);
        }


        /// <summary>
        /// Try to edit data. If any dont have any relevant values, then do not edit
        /// If do have relevant values, then edit that value
        /// If investment value doesnt exist, then add that value.
        /// </summary>
        public bool TryEditData(DateTime oldDate, DateTime newDate, double shares, double unitPrice, double Investment, IReportLogger reportLogger = null)
        {
            bool editShares = false;
            bool editUnitPrice = false;
            if (DoesDateSharesDataExist(oldDate, out int _))
            {
                editShares = fShares.TryEditData(oldDate, newDate, shares, reportLogger);
            }

            if (DoesDateUnitPriceDataExist(oldDate, out int _))
            {
                editUnitPrice = fUnitPrice.TryEditData(oldDate, newDate, unitPrice, reportLogger);
            }

            fInvestments.TryEditDataOtherwiseAdd(oldDate, newDate, Investment, reportLogger);

            return editShares & editUnitPrice && ComputeInvestments(reportLogger);
        }

        /// <summary>
        /// Edits name and company data of security.
        /// </summary>
        public bool EditNameData(NameData name)
        {
            Names = name;
            OnDataEdit(this, new EventArgs());
            return true;
        }

        internal bool TryRemoveSector(string sectorName)
        {
            if (IsSectorLinked(sectorName))
            {
                _ = Names.Sectors.Remove(sectorName);
                OnDataEdit(this, new EventArgs());
                return true;
            }

            return false;
        }

        internal bool TryAddSector(string sectorName)
        {
            if (!IsSectorLinked(sectorName))
            {
                _ = Names.Sectors.Add(sectorName);
                OnDataEdit(this, new EventArgs());
                return true;
            }

            return false;
        }

        public bool IsSectorLinked(string sectorName)
        {
            if (Names.Sectors != null && Names.Sectors.Any())
            {
                foreach (var name in Names.Sectors)
                {
                    if (name == sectorName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public int NumberSectors()
        {
            return Names.Sectors.Count;
        }

        /// <summary>
        /// Trys to get data on specific date. Only returns true if all data present.
        /// </summary>
        internal bool TryGetData(DateTime date, out double price, out double units, out double investment)
        {
            return fUnitPrice.TryGetValue(date, out price) & fShares.TryGetValue(date, out units) & fInvestments.TryGetValue(date, out investment);
        }

        /// <summary>
        /// Trys to get latest data earlier than date requested. Only returns true if all data present.
        /// </summary>
        internal bool TryGetEarlierData(DateTime date, out DailyValuation price, out DailyValuation units, out DailyValuation investment)
        {
            return fUnitPrice.TryGetNearestEarlierValue(date, out price) & fShares.TryGetNearestEarlierValue(date, out units) & fInvestments.TryGetNearestEarlierValue(date, out investment);
        }

        /// <summary>
        /// Tries to delete the data. If it can, it deletes all data specified, then returns true only if all data has been successfully deleted.
        /// </summary>
        public bool TryDeleteData(DateTime date, IReportLogger reportLogger = null)
        {
            return fUnitPrice.TryDeleteValue(date, reportLogger) & fShares.TryDeleteValue(date, reportLogger) & fInvestments.TryDeleteValue(date, reportLogger) && ComputeInvestments(reportLogger);
        }

        /// <summary>
        /// Upon a new/edit investment, one needs to recompute the values of the investments
        /// One should not change Inv = 0 or Inv > 0  to ensure that dividend reivestments are not accidentally included in a new investment.
        /// This though causes a problem if a value is deleted.
        /// </summary>
        /// <remarks>
        /// This should be called throughout, whenever one updates the data stored in the Security.
        /// </remarks>
        private bool ComputeInvestments(IReportLogger reportLogger = null)
        {
            // return true;
            for (int index = 0; index < fInvestments.Count(); index++)
            {
                var investmentValue = fInvestments[index];
                if (investmentValue.Value != 0)
                {
                    DailyValuation sharesCurrentValue = fShares.NearestEarlierValue(investmentValue.Day);
                    DailyValuation sharesPreviousValue = fShares.RecentPreviousValue(investmentValue.Day) ?? new DailyValuation(DateTime.Today, 0);
                    if (sharesCurrentValue != null)
                    {
                        fInvestments.TryEditData(investmentValue.Day, (sharesCurrentValue.Value - sharesPreviousValue.Value) * fUnitPrice.NearestEarlierValue(investmentValue.Day).Value, reportLogger);
                    }
                }
                if (investmentValue.Value == 0)
                {
                    if (fInvestments.TryDeleteValue(investmentValue.Day, reportLogger))
                    {
                        index--;
                    }
                }
            }

            return true;
        }
    }
}
