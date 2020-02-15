using FinancialStructures.ReportingStructures;
using System;
using System.Linq;

namespace FinancialStructures.DataStructures
{
    public partial class TimeList
    {
        /// <summary>
        /// Adds value to the data.
        /// </summary>
        private void AddData(DateTime date, double value, ErrorReports reports)
        {
            var valuation = new DailyValuation(date, value);
            fValues.Add(valuation);
            reports.AddReport($"Added value {value}", Location.AddingData);
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
        /// Checks if value on <param name="date"/> exists. If exists then index is output.
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
        internal bool TryAddValue(DateTime date, double value)
        {
            for (int i = 0; i < fValues.Count; i++)
            {
                if (fValues[i].Day == date)
                {
                    return false;
                }
            }

            AddData(date, value, new ErrorReports());
            return true;
        }

        /// <summary>
        /// Edits data on <paramref name="date"/> and replaces existing value with <paramref name="value"/>.
        /// </summary>
        internal bool TryEditData(DateTime date, double value, ErrorReports reports)
        {
            if (fValues != null && fValues.Any())
            {
                for (int i = 0; i < fValues.Count; i++)
                {
                    if (fValues[i].Day == date)
                    {
                        reports.AddReport($"Editing Data: {date} value changed from {fValues[i].Value} to {value}", Location.EditingData);
                        fValues[i].SetValue(value);

                        return true;
                    }
                }
            }

            return false;
        }

        internal bool TryEditData(DateTime oldDate, DateTime newDate, double value, ErrorReports reports)
        {
            if (fValues != null && fValues.Any())
            {
                for (int i = 0; i < fValues.Count; i++)
                {
                    if (fValues[i].Day == oldDate)
                    {
                        reports.AddReport($"Editing Data: {oldDate} value changed from {fValues[i].Value} to {newDate} - {value}", Location.EditingData);
                        fValues[i].SetData(newDate, value);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Edits the data on date specified. If data doesn't exist then adds the data.
        /// </summary>
        internal void TryEditDataOtherwiseAdd(DateTime oldDate, DateTime date, double value, ErrorReports reports)
        {
            if (!TryEditData(oldDate, date, value, reports))
            {
                AddData(date, value, reports);
            }
        }

        /// <summary>
        /// Deletes data if exists. If deletes, returns true.
        /// </summary>
        internal bool TryDeleteValue(DateTime date, ErrorReports reports)
        {
            if (fValues != null && fValues.Any())
            {
                for (int i = 0; i < fValues.Count; i++)
                {
                    if (fValues[i].Day == date)
                    {
                        reports.AddReport($"Deleted value: date - {date} and value - {fValues[i].Value}", Location.DeletingData);
                        fValues.RemoveAt(i);
                        return true;
                    }
                }
            }

            reports.AddError($"Deleting Value: Could not find data on date {date}.", Location.DeletingData);
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
