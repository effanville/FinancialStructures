using System;
using System.Linq;

namespace FinancialStructures.DataStructures
{
    public partial class TimeList
    {
        /// <summary>
        /// Adds value to the data.
        /// </summary>
        private void AddData(DateTime date, double value, Action<string, string, string> reportLogger)
        {
            var valuation = new DailyValuation(date, value);
            fValues.Add(valuation);
            reportLogger("Report", "AddingData", $"Added value {value}");
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
        internal bool TryAddValue(DateTime date, double value)
        {
            for (int i = 0; i < fValues.Count; i++)
            {
                if (fValues[i].Day == date)
                {
                    return false;
                }
            }
            void nothing(string a, string b, string c)
            {
                return;
            }
            AddData(date, value, nothing);
            return true;
        }

        /// <summary>
        /// Edits data on <paramref name="date"/> and replaces existing value with <paramref name="value"/>.
        /// </summary>
        internal bool TryEditData(DateTime date, double value, Action<string, string, string> reportLogger)
        {
            if (fValues != null && fValues.Any())
            {
                for (int i = 0; i < fValues.Count; i++)
                {
                    if (fValues[i].Day == date)
                    {
                        reportLogger("Report", "EditingData", $"Editing Data: {date} value changed from {fValues[i].Value} to {value}");
                        fValues[i].SetValue(value);

                        return true;
                    }
                }
            }

            return false;
        }

        internal bool TryEditData(DateTime oldDate, DateTime newDate, double value, Action<string, string, string> reportLogger)
        {
            if (fValues != null && fValues.Any())
            {
                for (int i = 0; i < fValues.Count; i++)
                {
                    if (fValues[i].Day == oldDate)
                    {
                        reportLogger("Report", "EditingData", $"Editing Data: {oldDate} value changed from {fValues[i].Value} to {newDate} - {value}");
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
        internal void TryEditDataOtherwiseAdd(DateTime oldDate, DateTime date, double value, Action<string, string, string> reportLogger)
        {
            if (!TryEditData(oldDate, date, value, reportLogger))
            {
                AddData(date, value, reportLogger);
            }
        }

        /// <summary>
        /// Deletes data if exists. If deletes, returns true.
        /// </summary>
        internal bool TryDeleteValue(DateTime date, Action<string, string, string> reportLogger)
        {
            if (fValues != null && fValues.Any())
            {
                for (int i = 0; i < fValues.Count; i++)
                {
                    if (fValues[i].Day == date)
                    {
                        reportLogger("Report", "DeletingData", $"Deleted value: date - {date} and value - {fValues[i].Value}");
                        fValues.RemoveAt(i);
                        return true;
                    }
                }
            }

            reportLogger("Error", "DeletingData", $"Deleting Value: Could not find data on date {date}.");
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
