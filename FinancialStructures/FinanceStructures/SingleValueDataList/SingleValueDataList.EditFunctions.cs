using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using StructureCommon.Reporting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public List<DayValue_ChangeLogged> GetDataForDisplay()
        {
            var output = new List<DayValue_ChangeLogged>();
            if (fValues.Any())
            {
                foreach (var datevalue in fValues.GetValuesBetween(fValues.FirstDate(), fValues.LatestDate()))
                {
                    fValues.TryGetValue(datevalue.Day, out double UnitPrice);
                    var thisday = new DayValue_ChangeLogged(datevalue.Day, UnitPrice, false);
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

        public List<object> CreateDataFromCsv(List<string[]> valuationsToRead, IReportLogger reportLogger = null)
        {
            var dailyValuations = new List<object>();
            foreach (var dayValuation in valuationsToRead)
            {
                if (dayValuation.Length != 2)
                {
                    reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Loading, "Line in Csv file has incomplete data.");
                    break;
                }

                var line = new DayValue_ChangeLogged(DateTime.Parse(dayValuation[0]), double.Parse(dayValuation[1]));
                dailyValuations.Add(line);
            }

            return dailyValuations;
        }

        public void WriteDataToCsv(TextWriter writer, IReportLogger reportLogger)
        {
            foreach (DayValue_ChangeLogged value in GetDataForDisplay())
            {
                writer.WriteLine(value.ToString());
            }
        }

        /// <summary>
        /// Edits value if value exists. Does nothing if it doesn't exist.
        /// </summary>
        public bool TryEditData(DateTime oldDate, DateTime date, double value, IReportLogger reportLogger = null)
        {
            return fValues.TryEditData(oldDate, date, value, reportLogger);
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
                Names.Sectors.Remove(sectorName);
                return true;
            }

            return false;
        }

        internal bool IsSectorLinked(string sectorName)
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
    }
}
