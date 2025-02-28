using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Effanville.Common.Structure.DataEdit;
using Effanville.Common.Structure.DataStructures;
using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.FinanceStructures.Implementation
{
    /// <summary>
    /// General edit functions for a sector.
    /// </summary>
    public partial class ValueList
    {
        /// <inheritdoc/>
        public virtual UpdateResult<NameData> EditNameData(NameData newNames)
        {
            if (!Names.Equals(newNames))
            {
                NameData oldName = Names.Copy();

                Names = newNames;
                OnDataEdit(this, new EventArgs());
                return UpdateResult.Change(oldName, Names.Copy());
            }

            return UpdateResult.Fail(Names, isChange: true);
        }

        /// <inheritdoc/>
        public virtual UpdateResult<DailyValuation> TryEditData(DateTime oldDate, DateTime date, decimal value)
        {
            if (Values.ValueExists(oldDate, out _))
            {
                return Values.TryEditData(oldDate, date, value);
            }

            return Values.SetData(date, value);
        }

        /// <inheritdoc/>
        public virtual UpdateResult<DailyValuation> SetData(DateTime date, decimal value)
            => Values.SetData(date, value);

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

                DailyValuation line = new DailyValuation(DateTime.Parse(dayValuation[0]), decimal.Parse(dayValuation[1]));
                dailyValuations.Add(line);
            }

            return dailyValuations;
        }

        /// <summary>
        /// Writes the data held in the account to a csv file.
        /// </summary>
        /// <param name="writer">The writer holding the location of where to write.</param>
        public virtual void WriteDataToCsv(TextWriter writer)
        {
            foreach (DailyValuation value in ListOfValues())
            {
                writer.WriteLine(value.ToString());
            }
        }

        /// <inheritdoc/>
        public virtual UpdateResult<DailyValuation> TryDeleteData(DateTime date) => Values.TryDeleteValue(date);

        /// <inheritdoc/>
        public bool TryRemoveSector(TwoName sectorName)
        {
            if (IsSectorLinked(sectorName.Name))
            {
                _ = Names.Sectors.Remove(sectorName.Name);
                OnDataEdit(this, new EventArgs());
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public bool IsSectorLinked(string identifier)
        {
            if (Names.Sectors != null && Names.Sectors.Any())
            {
                foreach (string name in Names.Sectors)
                {
                    if (name == identifier)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public int NumberSectors() => Names.Sectors.Count;
    }
}
