using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;
using StructureCommon.Reporting;

namespace FinancialStructures.FinanceStructures.Implementation
{
    /// <summary>
    /// General edit functions for a sector.
    /// </summary>
    public partial class ValueList
    {
        /// <inheritdoc/>
        public bool IsEqualTo(IValueList otherAccount)
        {
            return Names.IsEqualTo(otherAccount.Names);
        }

        /// <inheritdoc/>
        public int Count()
        {
            return Values.Count();
        }

        /// <summary>
        /// Retrieves data in a list ordered by date.
        /// </summary>
        public List<DailyValuation> GetDataForDisplay()
        {
            List<DailyValuation> output = new List<DailyValuation>();
            if (Values.Any())
            {
                foreach (DailyValuation datevalue in Values.GetValuesBetween(Values.FirstDate(), Values.LatestDate()))
                {
                    _ = Values.TryGetValue(datevalue.Day, out double UnitPrice);
                    DailyValuation thisday = new DailyValuation(datevalue.Day, UnitPrice);
                    output.Add(thisday);
                }
            }

            return output;
        }

        /// <inheritdoc/>
        public virtual bool EditNameData(NameData newNames)
        {
            Names = newNames;
            OnDataEdit(this, new EventArgs());
            return true;
        }

        /// <summary>
        /// Adds <param name="value"/> to amounts on <param name="date"/> if data doesnt exist.
        /// </summary>
        /// <param name="reportLogger">The logger to report outcomes.</param>
        public virtual bool TryAddData(DateTime date, double value, IReportLogger reportLogger = null)
        {
            if (Values.ValueExists(date, out _))
            {
                _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.AddingData, "Data already exists.");
                return false;
            }

            return Values.TryAddValue(date, value, reportLogger);
        }

        /// <summary>
        /// Adds <param name="value"/> to amounts on <param name="date"/> if data with date <param name="oldDate"/> doesnt exist, otherwise edits on that date.
        /// </summary>
        /// <param name="reportLogger">The logger to report outcomes.</param>
        public virtual bool TryAddOrEditData(DateTime oldDate, DateTime date, double value, IReportLogger reportLogger = null)
        {
            if (Values.ValueExists(oldDate, out _))
            {
                return Values.TryEditData(oldDate, date, value, reportLogger);
            }

            return Values.TryAddValue(date, value, reportLogger);
        }

        /// <summary>
        /// Adds data input already read from a csv file to the
        /// </summary>
        /// <param name="valuationsToRead">A list or array values of the data from the file.</param>
        /// <param name="reportLogger">A logger to record outcomes.</param>
        public virtual List<object> CreateDataFromCsv(List<string[]> valuationsToRead, IReportLogger reportLogger = null)
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

        /// <summary>
        /// Writes the data held in the account to a csv file.
        /// </summary>
        /// <param name="writer">The writer holding the location of where to write.</param>
        /// <param name="reportLogger">A logger to record outcomes.</param>
        public virtual void WriteDataToCsv(TextWriter writer, IReportLogger reportLogger = null)
        {
            foreach (DailyValuation value in GetDataForDisplay())
            {
                writer.WriteLine(value.ToString());
            }
        }

        /// <summary>
        /// Removes data on <paramref name="date"/> if it exists.
        /// </summary>
        public virtual bool TryDeleteData(DateTime date, IReportLogger reportLogger = null)
        {
            return Values.TryDeleteValue(date, reportLogger);
        }

        /// <summary>
        /// Removes a sector associated to this OldCashAccount.
        /// </summary>
        /// <param name="sectorName"></param>
        public bool TryRemoveSector(string sectorName)
        {
            if (IsSectorLinked(sectorName))
            {
                _ = Names.Sectors.Remove(sectorName);
                OnDataEdit(this, new EventArgs());
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public bool IsSectorLinked(string sectorName)
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

        /// <inheritdoc/>
        public int NumberSectors()
        {
            return Names.Sectors.Count;
        }
    }
}
