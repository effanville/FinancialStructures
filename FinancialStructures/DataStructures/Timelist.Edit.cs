using FinancialStructures.Reporting;
using System;
using System.Linq;

namespace FinancialStructures.DataStructures
{
    public partial class TimeList
    {
        /// <summary>
        /// Adds value to the data.
        /// </summary>
        private void AddData(DateTime date, double value, IReportLogger reportLogger = null)
        {
            var valuation = new DailyValuation(date, value);
            fValues.Add(valuation);
            OnDataEdit(this);
            _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.AddingData, $"Added value {value}");
            Sort();
        }

        /// <summary>
        /// Orders the list according to date.
        /// </summary>
        private void Sort()
        {
            if (fValues != null && fValues.Any())
            {
                fValues = fValues.OrderBy(x => x.Day).ToList();
            }
        }

        /// <summary>
        /// Checks if value on <param name="date"/> exists. If exists then <param name="index"/> is output.
        /// </summary>
        internal bool ValueExists(DateTime date, out int index)
        {
            if (fValues != null && fValues.Any())
            {
                for (int i = 0; i < fValues.Count; i++)
                {
                    if (fValues[i].Day == date)
                    {
                        index = i;
                        return true;
                    }
                }
            }

            index = -1;
            return false;
        }

        /// <summary>
        /// Adds value to the data only if value of the date doesn't currently exist.
        /// </summary>
        internal bool TryAddValue(DateTime date, double value, IReportLogger reportLogger = null)
        {
            for (int i = 0; i < fValues.Count; i++)
            {
                if (fValues[i].Day == date)
                {
                    return false;
                }
            }

            AddData(date, value, reportLogger);
            return true;
        }

        /// <summary>
        /// Edits data on <paramref name="date"/> and replaces existing value with <paramref name="value"/>.
        /// </summary>
        internal bool TryEditData(DateTime date, double value, IReportLogger reportLogger = null)
        {
            if (fValues != null && fValues.Any())
            {
                for (int i = 0; i < fValues.Count; i++)
                {
                    if (fValues[i].Day == date)
                    {
                        _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.EditingData, $"Editing Data: {date} value changed from {fValues[i].Value} to {value}");
                        OnDataEdit(this);
                        fValues[i].SetValue(value);

                        return true;
                    }
                }
            }

            return false;
        }

        internal bool TryEditData(DateTime oldDate, DateTime newDate, double value, IReportLogger reportLogger = null)
        {
            if (fValues != null && fValues.Any())
            {
                for (int i = 0; i < fValues.Count; i++)
                {
                    if (fValues[i].Day == oldDate)
                    {
                        _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.EditingData, $"Editing Data: {oldDate} value changed from {fValues[i].Value} to {newDate} - {value}");
                        fValues[i].SetData(newDate, value);
                        OnDataEdit(this);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Edits the data on date specified. If data doesn't exist then adds the data.
        /// </summary>
        internal void TryEditDataOtherwiseAdd(DateTime oldDate, DateTime date, double value, IReportLogger reportLogger = null)
        {
            if (!TryEditData(oldDate, date, value, reportLogger))
            {
                AddData(date, value, reportLogger);
            }
        }

        /// <summary>
        /// Deletes data if exists. If deletes, returns true.
        /// </summary>
        internal bool TryDeleteValue(DateTime date, IReportLogger reportLogger = null)
        {
            if (fValues != null && fValues.Any())
            {
                for (int i = 0; i < fValues.Count; i++)
                {
                    if (fValues[i].Day == date)
                    {
                        _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DeletingData, $"Deleted value: date - {date} and value - {fValues[i].Value}");
                        fValues.RemoveAt(i);
                        OnDataEdit(this);
                        return true;
                    }
                }
            }

            _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.DeletingData, $"Deleting Value: Could not find data on date {date}.");
            return false;
        }

        /// <summary>
        /// obtains first instance of the value for the date requested. Returns false if no data.
        /// </summary>
        internal bool TryGetValue(DateTime date, out double value)
        {
            value = 0;
            if (fValues != null && fValues.Any())
            {
                for (int i = 0; i < fValues.Count; i++)
                {
                    if (fValues[i].Day == date)
                    {
                        value = fValues[i].Copy().Value;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
