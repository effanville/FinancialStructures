﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;
using StructureCommon.Reporting;

namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// General edit functions for a sector.
    /// </summary>
    public partial class SingleValueDataList
    {
        /// <summary>
        /// Compares another security and determines if has same name and company.
        /// </summary>
        public bool IsEqualTo(ISingleValueDataList otherAccount)
        {
            return Names.IsEqualTo(otherAccount.Names);
        }

        public int Count()
        {
            return fValues.Count();
        }

        public List<DailyValuation> GetDataForDisplay()
        {
            List<DailyValuation> output = new List<DailyValuation>();
            if (fValues.Any())
            {
                foreach (DailyValuation datevalue in fValues.GetValuesBetween(fValues.FirstDate(), fValues.LatestDate()))
                {
                    _ = fValues.TryGetValue(datevalue.Day, out double UnitPrice);
                    DailyValuation thisday = new DailyValuation(datevalue.Day, UnitPrice);
                    output.Add(thisday);
                }
            }

            return output;
        }

        /// <summary>
        /// Edits the associated nameData to the account.
        /// </summary>
        /// <param name="newNames"></param>
        /// <returns></returns>
        public virtual bool EditNameData(NameData newNames)
        {
            Names = newNames;
            OnDataEdit(this, new EventArgs());
            return true;
        }

        /// <summary>
        /// Adds <param name="value"/> to amounts on <param name="date"/> if data doesnt exist.
        /// </summary>
        public bool TryAddData(DateTime date, double value, IReportLogger reportLogger = null)
        {
            if (fValues.ValueExists(date, out _))
            {
                _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.AddingData, "Data already exists.");
                return false;
            }

            return fValues.TryAddValue(date, value, reportLogger);
        }

        /// <summary>
        /// Adds <param name="value"/> to amounts on <param name="date"/> if data doesnt exist.
        /// </summary>
        public bool TryAddOrEditData(DateTime oldDate, DateTime date, double value, IReportLogger reportLogger = null)
        {
            if (fValues.ValueExists(oldDate, out _))
            {
                return fValues.TryEditData(oldDate, date, value, reportLogger);
            }

            return fValues.TryAddValue(date, value, reportLogger);
        }

        public List<object> CreateDataFromCsv(List<string[]> valuationsToRead, IReportLogger reportLogger = null)
        {
            List<object> dailyValuations = new List<object>();
            foreach (string[] dayValuation in valuationsToRead)
            {
                if (dayValuation.Length != 2)
                {
                    _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Loading, "Line in Csv file has incomplete data.");
                    break;
                }

                DailyValuation line = new DailyValuation(DateTime.Parse(dayValuation[0]), double.Parse(dayValuation[1]));
                dailyValuations.Add(line);
            }

            return dailyValuations;
        }

        public void WriteDataToCsv(TextWriter writer, IReportLogger reportLogger)
        {
            foreach (DailyValuation value in GetDataForDisplay())
            {
                writer.WriteLine(value.ToString());
            }
        }

        /// <summary>
        /// Removes data on <paramref name="date"/> if it exists.
        /// </summary>
        public bool TryDeleteData(DateTime date, IReportLogger reportLogger = null)
        {
            return fValues.TryDeleteValue(date, reportLogger);
        }

        /// <summary>
        /// Removes a sector associated to this OldCashAccount.
        /// </summary>
        /// <param name="sectorName"></param>
        /// <returns></returns>
        public bool TryRemoveSector(string sectorName)
        {
            if (IsSectorLinked(sectorName))
            {
                _ = Names.Sectors.Remove(sectorName);
                return true;
            }

            return false;
        }

        internal bool IsSectorLinked(string sectorName)
        {
            if (Names.Sectors != null && Names.Sectors.Any())
            {
                foreach (string name in Names.Sectors)
                {
                    if (name == sectorName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}